﻿using System.Collections.Generic;
using UnityEngine;
using helloVoRld.Test.Databases;
using helloVoRld.Core.Singletons;
using helloVoRld.Test.UI;
using helloVoRld.Utilities.Debugging;
using helloVoRld.Utilities;
using helloVoRld.Core.Pooling;
using helloVoRld.Networking.RestClient;

namespace helloVoRld.Test.Managers
{
    [RequireComponent(typeof(ObjectPooler))]
    public class CatalogueManager : Singleton<CatalogueManager>
    {
        #region variables
        public List<S_Catalogue> Catalogues => CatalogueClient.Instance.Catalogues;

        [Header("Database")]
        public float loadingProgress;

        [Header("UI References")]
        public RectTransform catalogueScrollContentHolder;


        [Header("Pooling")]
        public int buttonsToPool;

        // private variables
        private ObjectPooler catalogueButtonPooler;
        #endregion

        #region main code
        private void Start()
        {
            catalogueButtonPooler = GetComponent<ObjectPooler>();
            catalogueButtonPooler.InitializePool(buttonsToPool);
            if (catalogueButtonPooler.isPoolInitialized)
            {
                CatalogueClient.Instance.GetCatalogues(() => { PopulateCatalogues(); });
            }
        }

        #endregion

        #region helper code

        private void PopulateCatalogues()
        {
            TransformUtils.ClearChilds(catalogueScrollContentHolder, catalogueButtonPooler);
            loadingProgress = 0;

            if (Catalogues.Count > 0)
            {
                for (int i = 0; i < Catalogues.Count; i++)
                {
                    // Pooling
                    GameObject catalogueButtonGO = catalogueButtonPooler.GetObject();
                    catalogueButtonGO.transform.SetParent(catalogueScrollContentHolder);
                    catalogueButtonGO.GetComponent<RectTransform>().localScale = Vector3.one;
                    //Initialize
                    bool success = catalogueButtonGO.GetComponentInChildren<CatalogueButton>().Init(Catalogues[i], i);

                    // SetLoadProgress
                    if (success)
                    {
                        SetLoadProgress(i);
                    }
                }
            }
            else
            {
                DebugHelper.Log("Couldn't load catalogue buttons");
            }
        }

        private void SetLoadProgress(int indexLoaded)
        {
            loadingProgress = ((float)(indexLoaded + 1) / Catalogues.Count) * 100;
        }



        #endregion
    }
}