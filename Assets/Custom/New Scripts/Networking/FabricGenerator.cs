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
    public class FabricGenerator : Singleton<FabricGenerator>
    {
        static string IP => Globals.IP;
        static List<CatalogueModel> CatalogueModels => Globals.Catalogues;

        public void LoadFabrics(int CatIndex, Action<List<FabricModel>> OnSuccess, Action OnFailure)
        {
            if (CatIndex >= CatalogueModels.Count || CatIndex < 0)
                throw new Exception("Invalid index passed for fabric: " + CatIndex);


            if (CatalogueModels[CatIndex].FabricList.Count != 0)
            {
                OnSuccess(CatalogueModels[CatIndex].FabricList);
                return;
            }

            StartCoroutine(RestWebClient.Instance.HttpGet(IP + @"/fabricapp/api/fabrics?catpk=" + CatalogueModels[CatIndex].WebID, (response) =>
            {
                if (response.Error != null)
                {
                    OnFailure();
                    return;
                }
                else
                {
                    var list = JsonConvert.DeserializeObject<List<FabricWebModel>>(response.Data);
                    foreach (var fabric in list)
                    {
                        CatalogueModels[CatIndex].FabricList.Add(new FabricModel(fabric));
                    }

                    OnSuccess(CatalogueModels[CatIndex].FabricList);
                }
            },
            new[] { Globals.RequestHeader }));
        }

    }
}
