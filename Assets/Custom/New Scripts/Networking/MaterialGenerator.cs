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
        /// <summary>
        /// int: WEBID, string: JSON
        /// </summary>
        readonly Dictionary<int, string> Materials = new Dictionary<int, string>();
        /// <summary>
        /// string: URL, Texture2D: Texture
        /// </summary>
        readonly Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        /// <summary>
        /// Used to deserialize JSON
        /// </summary>
        public readonly MaterialWrapper MaterialDeserializer = new MaterialWrapper();

        FurnitureModel Furniture => Globals.SelectedFurniture;
        CatalogueModel Catalogue => Globals.SelectedCatalogue;
        FabricModel Fabric => Globals.SelectedFabric;

        readonly Dictionary<string, List<Action<Texture2D>>> CallbackHistory = new Dictionary<string, List<Action<Texture2D>>>();

        /// <summary>
        /// Should be called from Fabric Button, otherwise all references may not be set
        /// </summary>
        /// <param name="OnSuccess">After creating material with </param>
        public void GetAppropriateMaterial(Action<Material> OnSuccess)
        {
            if ((Furniture == null || Catalogue == null || Fabric == null) && Globals.InitialFabricLoaded)
                throw new Exception("Developer Error - This code shold not be called at this stage");

            GetAppropriateMaterial(Fabric.MaterialIndex, Fabric.MainTexURL, Catalogue.NormalMapURL, Fabric.DateSuffix, OnSuccess);
        }

        public void GetAppropriateMaterial(int matIndex, string MainTextureURL, string NormalMAPURL, string Date, Action<Material> OnSuccess)
        {
            StartCoroutine(MaterialRequest(matIndex, (material) =>
            {
                if (MainTextureURL != "" && MainTextureURL != null)
                {
                    StartCoroutine(TexturesRequest(MainTextureURL, Date, texture =>
                    {
                        material.SetTexture("_MainTex", texture);
                    }));
                }

                if (NormalMAPURL != "" && NormalMAPURL != null)
                {
                    StartCoroutine(TexturesRequest(NormalMAPURL, Date, texture =>
                    {
                        material.SetTexture("_NormTex", texture);
                    }));
                }

                OnSuccess(material);
            }));
        }

        IEnumerator TexturesRequest(string url, string Date, Action<Texture2D> TexturesOnSuccess)
        {
            // If Texture is already loaded
            if (Textures.ContainsKey(url))
            {
                TexturesOnSuccess(Textures[url]);
            }

            // If texture is present on disk
            else if (Globals.IsTextureOnDisk(url, Date, out Texture2D t))
            {
                Textures.Add(url, t);
                TexturesOnSuccess(t);
            }

            // If a request for texture is already made
            else if (CallbackHistory.ContainsKey(url))
            {
                CallbackHistory[url].Add(TexturesOnSuccess);
            }

            // Request for texture from WEB
            else
            {
                CallbackHistory.Add(url, new List<Action<Texture2D>> { TexturesOnSuccess });
                yield return RestWebClient.Instance.HttpDownloadImage(url,
                  response =>
                  {
                      Texture2D tex = response.textureDownloaded;
                      Textures.Add(url, tex);
                      Globals.WriteTextureOnDisk(url, Date, tex);

                      foreach (var x in CallbackHistory[url])
                          x(tex);

                      CallbackHistory.Remove(url);
                  },
                  progress => { });
            }
        }

        IEnumerator MaterialRequest(int index, Action<Material> MaterialOnSuccess)
        {
            // JSON Deserializor
            Material GetFromJson(string JSON)
            {
                MaterialDeserializer.DeserializeValues(JsonConvert.DeserializeObject<Dictionary<string, object>>(JSON));
                return MaterialDeserializer.MainObject;
            }

            // Already Present in List
            if (Materials.ContainsKey(index))
            {
                MaterialOnSuccess(GetFromJson(Materials[index]));
                yield return null;
            }

            // Load from Web
            else yield return RestWebClient.Instance.HttpGet(Globals.MaterialIP(index), response =>
            {
                if (response.IsValidResponse)
                {
                    var material = GetFromJson(response.Data);

                    if (!Materials.ContainsKey(index))
                        Materials.Add(index, response.Data);
                    MaterialOnSuccess(material);
                }
            },
            new[] { Globals.RequestHeader });
        }
    }
}