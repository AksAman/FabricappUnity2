using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using helloVoRld.Networking.RestClient;
using helloVoRld.Networking.Models;
using helloVoRld.Core.Singletons;
using Newtonsoft.Json;
using UnityEngine;
using helloVoRld.Test.Databases;
using helloVoRld.NewScripts.Catalogue;

namespace helloVoRld.Networking.RestClient
{
    public class WebClient : Singleton<WebClient>
    {
        enum State
        {
            RequestNotSent,
            RequestSent,
            RequestCompleted
        }

        readonly List<Action<List<CatalogueModel>>> QueueList = new List<Action<List<CatalogueModel>>>();
        private State CurState = State.RequestNotSent;

        readonly string IP = @"http://hvrplfabricapp.ml";
        readonly RequestHeader requestHeader = new RequestHeader { Key = "Authorization", Value = "Token " + "0ef442a637f1570b5f848f164ee972219eaca8bc" };

        [Obsolete]
        public readonly List<S_Catalogue> Catalogues = new List<S_Catalogue>();

        readonly List<CatalogueModel> CatalogueModels = new List<CatalogueModel>();

        public void GetCatalogues(Action<List<CatalogueModel>> OnSuccess)
        {
            if (CurState == State.RequestCompleted)
                OnSuccess(CatalogueModels);
            else if (CurState == State.RequestSent)
                QueueList.Add(OnSuccess);
            else
            {
                CurState = State.RequestSent;
                QueueList.Add(OnSuccess);
                StartCoroutine(RestWebClient.Instance.HttpInternetCheck(IP + "/fabricapp/",
                    OnSuccess: () =>
                    {
                        Debug.Log("Internet Available");
                        StartCoroutine(CatalogueRequest());
                    },
                    OnFail: () =>
                     {
                         Debug.Log("No Internet!!!");
                     }));
            }
        }

        IEnumerator CatalogueRequest()
        {
            return RestWebClient.Instance.HttpGet(IP + @"/fabricapp/api/catalogues", (response) =>
            {
                if (response.Error == "" || response.Error == null)
                {
                    var list = JsonConvert.DeserializeObject<List<CatalogueWebModel>>(response.Data);

                    CatalogueModels.AddRange(from x in list select new CatalogueModel(x));

                    LoadFabrics(0, OnSuccess: () =>
                    {
                        CurState = State.RequestCompleted;
                        for (int i = 0; i < QueueList.Count; ++i)
                        {
                            QueueList[i](CatalogueModels);
                            QueueList[i] = null;
                        }
                        QueueList.Clear();
                    },
                    OnFailure: () =>
                    {

                    });
                }
            }, new[] { requestHeader });
        }

        public void LoadFabrics(int CatIndex, Action OnSuccess, Action OnFailure)
        {
            if (CatIndex >= CatalogueModels.Count || CatIndex < 0)
                throw new Exception("Invalid index passed for fabric: " + CatIndex);

            OnSuccess();
            /*
            if (Catalogues[CatIndex].c_fabrics != null && Catalogues[CatIndex].c_fabrics.Count != 0)
            {
                OnSuccess();
                return;
            }

            Catalogues[CatIndex].c_fabrics = new List<Fabric>();
            StartCoroutine(RestWebClient.Instance.HttpGet(IP + @"/fabricapp/api/fabrics?catpk=" + Catalogues[CatIndex].WEB_Id, (response) =>
            {
                if (response.Error != null)
                {
                    OnFailure();
                    return;
                }
                else
                {
                    var responseCatalog = new[]
                    {
                        new
                        {
                            id = 0,
                            f_name = "",
                            f_thumbnail_url = "",
                            f_fabric_texture = "",
                            catalogue = 0,
                            f_material = 0
                        }
                    }.ToList();

                    var list = JsonConvert.DeserializeAnonymousType(response.Data, responseCatalog);
                    foreach (var fabrics in list)
                    {
                        Catalogues[CatIndex].c_fabrics.Add(new Fabric
                        {
                            f_title = fabrics.f_name,
                            f_thumbnail_url = IP + fabrics.f_thumbnail_url,
                            f_material = null
                        });
                    }

                    OnSuccess();
                }
            },
            new[] { requestHeader }));*/
        }
    }
}