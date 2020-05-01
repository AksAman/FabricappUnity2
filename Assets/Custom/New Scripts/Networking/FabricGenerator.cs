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
        /// <summary>
        /// Load fabrics based on the catalogue index defined in scrollview
        /// </summary>
        /// <param name="CatIndex">Local Catalogue Index</param>
        /// <param name="OnSuccess">Callback when Fabric List is loaded</param>
        /// <param name="OnFailure">Callback for Fabric List Loading Error</param>
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

        public void LoadInitialFabric(Action<FabricModel> OnSuccess)
        {

        }
    }
}