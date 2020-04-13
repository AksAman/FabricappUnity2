using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using helloVoRld.Networking.Models;
using helloVoRld.NewScripts.Engine;
using Newtonsoft.Json;
using UnityEngine;
using helloVoRld.Test.Databases;
using helloVoRld.NewScripts.Catalogue;
using helloVoRld.NewScripts.Fabric;

namespace helloVoRld.Networking
{
    public class CatalogueGenerator : Singleton<CatalogueGenerator>
    {
        static string IP => Globals.IP;
        static List<CatalogueModel> Catalogues => Globals.Catalogues;

        public void GetCatalogues(Action<List<CatalogueModel>> OnSuccess)
        {
            if (Catalogues.Count > 0)
            {
                OnSuccess(Catalogues);
                return;
            }

            StartCoroutine(RestWebClient.Instance.HttpInternetCheck(IP + "/fabricapp/",
                OnSuccess: () =>
                {
                    Debug.Log("Internet Available");
                    StartCoroutine(CatalogueRequest(OnSuccess));
                },
                OnFail: () =>
                {
                    Debug.Log("No Internet!!!");
                }));
        }

        IEnumerator CatalogueRequest(Action<List<CatalogueModel>> OnSuccess)
        {
            return RestWebClient.Instance.HttpGet(IP + @"/fabricapp/api/catalogues", response =>
            {
                if (response.IsValidResponse)
                {
                    var list = JsonConvert.DeserializeObject<List<CatalogueWebModel>>(response.Data);

                    Catalogues.AddRange(from x in list select new CatalogueModel(x));

                    OnSuccess(Catalogues);
                }
            }, new[] { Globals.RequestHeader });
        }
    }

    #region OLD Code
    /*
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

        List<CatalogueModel> CatalogueModels => Globals.Catalogues;

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

                    CurState = State.RequestCompleted;
                    for (int i = 0; i < QueueList.Count; ++i)
                    {
                        QueueList[i](CatalogueModels);
                        QueueList[i] = null;
                    }
                    QueueList.Clear();
                }
            }, new[] { requestHeader });
        }
*/
    #endregion
}