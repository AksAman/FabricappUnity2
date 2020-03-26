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

namespace helloVoRld.Networking.RestClient
{
    internal class FixedCountDownloader : Singleton<FixedCountDownloader>
    {
        readonly IEnumerator[] Routines = new IEnumerator[1];
        readonly Queue<(string, Action<Sprite>)> Queue = new Queue<(string, Action<Sprite>)>();
        
        void Update()
        {
            if (Queue.Count != 0)
            {
                int emptyIndex = Array.IndexOf(Routines, default);

                if (emptyIndex == -1)
                    return;

                (var Path, var Action) = Queue.Dequeue();
                Routines[emptyIndex] = RestWebClient.Instance.HttpDownloadImage(Path, (response, _) =>
                {
                    // Clear Array Index
                    Routines[emptyIndex] = default;
                    
                    Texture2D tex = response.textureDownloaded;
                    if (response.Error != "")
                        return;

                    Sprite s = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    
                    // Do Whatever to do with sprite
                    Action(s);
                }, 0);
                StartCoroutine(Routines[emptyIndex]);
            }
        }

        public void AddTask(string path, Action<Sprite> Act)
        {
            Queue.Enqueue((path, Act));
        }
    }

    public class CatalogueClient : Singleton<CatalogueClient>
    {
        enum State
        {
            RequestNotSent,
            RequestSent,
            RequestCompleted
        }
        private readonly List<Action> OnSuccessWaiters = new List<Action>();
        private State CurState = State.RequestNotSent;

        readonly string IP = @"http://hvrplfabricapp.ml";
        readonly RequestHeader requestHeader = new RequestHeader { Key = "Authorization", Value = "Token " + "0ef442a637f1570b5f848f164ee972219eaca8bc" };

        public readonly List<S_Catalogue> Catalogues = new List<S_Catalogue>();

        public void Awake()
        {/*
            StartCoroutine(RestWebClient.Instance.HttpGet(IP + @"/fabricapp/api/rest-auth/login", (response) =>
            {
                Debug.Log("Key : " + response.Error + " --- " +  response.Data);
            }));*/
        }

        public void GetCatalogues(Action OnSuccess)
        {
            if (CurState == State.RequestCompleted)
                OnSuccess();
            else if (CurState == State.RequestSent)
                OnSuccessWaiters.Add(OnSuccess);
            else
            {
                CurState = State.RequestSent;
                OnSuccessWaiters.Add(OnSuccess);
                StartCoroutine(RestWebClient.Instance.HttpGet(IP + @"/fabricapp/api/catalogues", (response) =>
                {
                    if (response.Error == "" || response.Error == null)
                    {
                        var responseCatalog = new[]
                        {
                            new
                            {
                                id = 0,
                                c_name = "",
                                c_description = "",
                                c_thumbnail_url = "",
                                c_manufacturer_name = "",
                                c_normal_map = ""
                            }
                        }.ToList();

                        var list = JsonConvert.DeserializeAnonymousType(response.Data, responseCatalog);

                        foreach (var catalog in list)
                        {
                            S_Catalogue cat = new S_Catalogue
                            {
                                WEB_Id = catalog.id,
                                c_name = catalog.c_name,
                                c_description = catalog.c_description,
                                c_thumbnail_url = IP + catalog.c_thumbnail_url,
                                manufacturer_name = catalog.c_manufacturer_name,
                                c_fabrics = null
                            };
                            Catalogues.Add(cat);
                        }

                        LoadFabrics(0, OnSuccess: () =>
                        {
                            CurState = State.RequestCompleted;
                            for (int i = 0; i < OnSuccessWaiters.Count; ++i)
                            {
                                OnSuccessWaiters[i]();
                                OnSuccessWaiters[i] = null;
                            }
                            OnSuccessWaiters.Clear();
                        },
                        OnFailure: () => 
                        {
                            
                        });
                    }
                }, new[] { requestHeader }));
            }
        }

        public void LoadFabrics(int CatIndex, Action OnSuccess, Action OnFailure)
        {
            if (CatIndex >= Catalogues.Count || CatIndex < 0)
                throw new Exception("Invalid index passed for fabric: " + CatIndex);

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
                            f_thumbnail = null,
                            f_material = null
                        });
                    }

                    OnSuccess();
                }
            },
            new[] { requestHeader }));
        }

    }
}