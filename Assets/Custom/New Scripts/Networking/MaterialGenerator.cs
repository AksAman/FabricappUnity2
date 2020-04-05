using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using helloVoRld.NewScripts.Catalogue;
using helloVoRld.NewScripts.Furniture;
using helloVoRld.NewScripts.Fabric;
using UnityEngine;
using helloVoRld.Core;
using helloVoRld.NewScripts.Engine;
using Newtonsoft.Json;

namespace helloVoRld.Networking
{
    public class MaterialGenerator : Singleton<MaterialGenerator>
    {
        Dictionary<FabricModel, Material> Materials = new Dictionary<FabricModel, Material>();
        Dictionary<string, Texture2D> TextureList = new Dictionary<string, Texture2D>();

        FurnitureModel Furniture => Globals.SelectedFurniture;
        CatalogueModel Catalogue => Globals.SelectedCatalogue;
        FabricModel Fabric => Globals.SelectedFabric;

        public void GetAppropriateMaterial(Action<Material> OnSuccess)
        {
            if (Furniture == null || Catalogue == null || Fabric == null)
                throw new Exception("Developer Error - This code shold not be called at this stage");

            Material material = Materials[Fabric];
            if (material == null)
            {

            }
            
        }

        IEnumerator MaterialRequest(string url, Action<Material> MaterialOnSuccess)
        {
            if (Materials[Fabric] != null)
            {
                MaterialOnSuccess(Materials[Fabric]);
                yield return null;
            }
            else yield return RestWebClient.Instance.HttpGet(url, response => 
            {
                if (response.IsValidResponse)
                {

                }
            });
        }

        Material DeserializeJSON(string JSON)
        {
            return null;
        }
    }
}
