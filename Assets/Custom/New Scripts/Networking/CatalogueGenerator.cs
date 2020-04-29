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
}