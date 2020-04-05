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
using helloVoRld.NewScripts.Wrappers;

namespace helloVoRld.Networking
{
    public class MaterialGenerator : Singleton<MaterialGenerator>
    {
        readonly Dictionary<int, Material> Materials = new Dictionary<int, Material>();
        readonly Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        readonly MaterialWrapper MaterialDeserializer = new MaterialWrapper();

        FurnitureModel Furniture => Globals.SelectedFurniture;
        CatalogueModel Catalogue => Globals.SelectedCatalogue;
        FabricModel Fabric => Globals.SelectedFabric;

        public void GetAppropriateMaterial(Action<Material> OnSuccess)
        {
            if (Furniture == null || Catalogue == null || Fabric == null)
                throw new Exception("Developer Error - This code shold not be called at this stage");

            StartCoroutine(MaterialRequest(Fabric.MaterialIndex, (material) =>
            {
                Materials.Add(Fabric.MaterialIndex, material);

                if (Fabric.MainTexURL != "" && Fabric.MainTexURL != null)
                {
                    StartCoroutine(TexturesRequest(Fabric.MainTexURL, texture => 
                    {
                        material.SetTexture("_MainTex", texture);
                    }));
                }

                if (Catalogue.NormalMapURL != "" && Catalogue.NormalMapURL != null)
                {
                    StartCoroutine(TexturesRequest(Catalogue.NormalMapURL, texture =>
                    {
                        material.SetTexture("_NormTex", texture);
                    }));
                }

                OnSuccess(material);
            }));
        }

        IEnumerator TexturesRequest(string url, Action<Texture2D> TexturesOnSuccess)
        {
            if (Textures.ContainsKey(url))
            {
                TexturesOnSuccess(Textures[url]);
                yield return null;
            }
            else yield return RestWebClient.Instance.HttpDownloadImage(url,
                response =>
                {
                    Texture2D tex = response.textureDownloaded;
                    Textures.Add(url, tex);
                    TexturesOnSuccess(tex);
                },
                progress => Debug.Log(progress));
        }

        IEnumerator MaterialRequest(int index, Action<Material> MaterialOnSuccess)
        {
            if (Materials.ContainsKey(index))
            {
                MaterialOnSuccess(Materials[index]);
                yield return null;
            }
            else yield return RestWebClient.Instance.HttpGet(Globals.MaterialIP(index), response =>
            {
                if (response.IsValidResponse)
                {
                    MaterialDeserializer.DeserializeValues(JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Data));
                    var material = MaterialDeserializer.MainObject;
                    Materials.Add(index, material);
                    MaterialOnSuccess(material);
                }
            },
            new[] { Globals.RequestHeader });
        }
    }
}
