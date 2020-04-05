/*using helloVoRld.Core.Singletons;
using helloVoRld.Test.UI;
using helloVoRld.Test.Databases;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using helloVoRld.Utilities;
using helloVoRld.Core.Pooling;
using helloVoRld.Utilities.Debugging;
using helloVoRld.Networking;

namespace helloVoRld.Test.Managers
{
    [RequireComponent(typeof(ObjectPooler))]
    public class FabricsManager : Singleton<FabricsManager>
    {
        #region variables
        // public
        [Header("UI References")]
        public RectTransform fabricScrollContentHolder;
        public GameObject cataloguePanel;
        public GameObject fabricsPanel;
        public TextMeshProUGUI catalogueTitleText;
        public TextMeshProUGUI catalogueDescriptionText;
        public float loadingProgress;

        [Header("Pooling")]
        public int buttonsToPool;

        // Fields
        public int CurrentCatalogueIndex { get => currentCatalogueIndex; private set => currentCatalogueIndex = value; }
        public FixedCountDownloader TextureDownloader;


        // private
        private ObjectPooler fabricButtonPooler;
        private CatalogueManager catalogueManager;
        private int currentCatalogueIndex;
        private UIStateSystem uistatesystem;

        #endregion


        #region main code

        private void Awake()
        {
            TextureDownloader = new FixedCountDownloader(this);
        }
        
        private void Start()
        {
            uistatesystem = UIStateSystem.Instance;

            fabricButtonPooler = GetComponent<ObjectPooler>();
            fabricButtonPooler.InitializePool(buttonsToPool);
            if (fabricButtonPooler.isPoolInitialized)
            {
                catalogueManager = CatalogueManager.Instance;

                CatalogueButton.OnCatalogueButtonClicked += CatalogueButtonClicked;

            }

        }

        private void Update()
        {
            TextureDownloader.Update();
        }

        #endregion


        #region helper code

        private void CatalogueButtonClicked(int catalogueButtonIndex)
        {
            CurrentCatalogueIndex = catalogueButtonIndex;
            DebugHelper.Log(catalogueButtonIndex.ToString());
            uistatesystem.ShowLoadingScreen();
            TextureDownloader.StopAll();
            WebClient.Instance.LoadFabrics(catalogueButtonIndex,
                OnSuccess: (abc) =>
                {
                    uistatesystem.RemoveLoadingScreen();
                    // Populate Fabric panel based on catalogues[i].fabrics
                    PopulateFabrics(catalogueManager.Catalogues[catalogueButtonIndex]);

                    // Switch to FabricPanel
                    cataloguePanel.SetActive(false);
                    fabricsPanel.SetActive(true);
                },
                OnFailure:
                () =>
                {
                    uistatesystem.RemoveLoadingScreen();
                });
        }



        public void PopulateFabrics(S_Catalogue catalogue)
        {
            catalogueTitleText.text = catalogue.c_name;
            catalogueDescriptionText.text = catalogue.c_description;

            TransformUtils.ClearChilds(fabricScrollContentHolder, fabricButtonPooler);
            loadingProgress = 0;
            List<Fabric> fabrics = catalogue.c_fabrics;
            for (int i = 0; i < fabrics.Count; i++)
            {
                GameObject fabricButtonGO = fabricButtonPooler.GetObject();
                fabricButtonGO.transform.SetParent(fabricScrollContentHolder);
                fabricButtonGO.GetComponent<RectTransform>().localScale = Vector3.one;
                //Initialize
                bool success = fabricButtonGO.GetComponentInChildren<FabricButton>().Init(fabrics[i], i);

                // SetLoadProgress
                if (success)
                {
                    SetLoadProgress(i);
                }
            }
        }

        private void SetLoadProgress(int indexLoaded)
        {
            loadingProgress = ((float)(indexLoaded + 1) / catalogueManager.Catalogues.Count) * 100;
        }

        #endregion
    }
}
*/