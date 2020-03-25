using helloVoRld.Core.Singletons;
using System.Collections.Generic;
using UnityEngine;
using helloVoRld.Test.Databases;
using helloVoRld.Core.Pooling;
using helloVoRld.Utilities;
using helloVoRld.Test.UI;
using helloVoRld.Utilities.Debugging;
using TMPro;
using helloVoRld.Networking.RestClient;

namespace helloVoRld.Test.Managers
{
    [RequireComponent(typeof(ObjectPooler))]
    public class ModelManager : Singleton<ModelManager>
    {
        #region Variables
        // public
        [Header("Database")]
        public List<Model> models = new List<Model>();

        // Fields
        public GameObject CurrentModel { get => currentModel; private set => currentModel = value; }
        public int CurrentModelIndex { get => currentModelIndex; private set => currentModelIndex = value; }

        //private
        [Header("Scene References")]
        [SerializeField] private Transform modelHolder;
        [SerializeField] private RectTransform furnitureScrollContentHolder;
        [SerializeField] private TextMeshProUGUI currentFurnitureText;

        [Header("Pooling")]
        [SerializeField] private int buttonsToPool;

        [SerializeField] private bool rememberMaterials;
        private Dictionary<int, Dictionary<string, int>> modelMaterialReferences = new Dictionary<int, Dictionary<string, int>>();
        private Dictionary<int, bool> isModelSpawned = new Dictionary<int, bool>();
        private float loadingProgress = 0;
        private GameObject currentModel;
        private ObjectPooler furnitureButtonPooler;
        private List<GameObject> spawnedModels = new List<GameObject>();
        private int currentModelIndex;
        private MaterialManager materialManager;
        private CatalogueManager catalogueManager;
        private FabricsManager fabricsManager;
        #endregion


        #region Main functions
        private void Start()
        {
            materialManager = MaterialManager.Instance;
            catalogueManager = CatalogueManager.Instance;
            fabricsManager = FabricsManager.Instance;
            furnitureButtonPooler = GetComponent<ObjectPooler>();
            furnitureButtonPooler.InitializePool(buttonsToPool);

            CatalogueClient.Instance.GetCatalogues(() =>
            {
                if (furnitureButtonPooler.isPoolInitialized)
                {
                    PopulateFurnitureButtons();
                }

                FurnitureButton.OnFurnitureButtonClicked += FurnitureButtonClicked;

                if (models.Count > 0)
                {
                    ChangeFurniture(0);
                }

                if (rememberMaterials)
                {
                    FabricButton.OnFabricButtonClicked += AssignModelMaterialReference;
                }
            });
            
        }

        #endregion


        #region Helper Functions

        private void FurnitureButtonClicked(int furnitureButtonIndex)
        {
            //SubMeshTriangle.clickedMatIndex = 0;
            
            ChangeFurniture(furnitureButtonIndex);
        }

        private void UpdateCurrentFurnitureText(int furnitureButtonIndex)
        {
            currentFurnitureText.text = $"Furniture : {models[furnitureButtonIndex].modelName}".ToUpper();
        }

        private void PopulateFurnitureButtons()
        {
            TransformUtils.ClearChilds(furnitureScrollContentHolder, furnitureButtonPooler);
            loadingProgress = 0;
            int modelsCount = models.Count;
            if (modelsCount > 0)
            {
                for (int i = 0; i < modelsCount; i++)
                {
                    //Instantiate
                    GameObject catalogueButtonGO = furnitureButtonPooler.GetObject();
                    catalogueButtonGO.transform.SetParent(furnitureScrollContentHolder);
                    catalogueButtonGO.GetComponent<RectTransform>().localScale = Vector3.one;
                    //Initialize
                    bool success = catalogueButtonGO.GetComponentInChildren<FurnitureButton>().Init(models[i], i);

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
            loadingProgress = ((float)(indexLoaded + 1) / models.Count) * 100;
        }

        private void ChangeFurniture(int newIndex)
        {
            if(CurrentModel)
            {
                Destroy(CurrentModel);
            }

            CurrentModelIndex = newIndex;
            UpdateCurrentFurnitureText(CurrentModelIndex);

            GameObject spawnedModelGO = Instantiate(models[CurrentModelIndex].modelPrefab, modelHolder);
            CurrentModel = spawnedModelGO;

            if(rememberMaterials)
            {
                //Apply last remembered material
                if (modelMaterialReferences.ContainsKey(CurrentModelIndex))
                {
                    int catalogueIndex = modelMaterialReferences[CurrentModelIndex]["Catalogue"];
                    int fabricIndex = modelMaterialReferences[CurrentModelIndex]["Fabric"];
                    materialManager.ChangeCurModelMaterial(models[CurrentModelIndex].materialIndexToChange, catalogueIndex, fabricIndex);
                }

            }
            else
            {
                int catalogueIndex = fabricsManager.CurrentCatalogueIndex;
                int fabricIndex = materialManager.CurrentFabricIndex;
                materialManager.ChangeCurModelMaterial(models[CurrentModelIndex].materialIndexToChange, catalogueIndex, fabricIndex);
            }

            // Set gameobject active
            spawnedModelGO.SetActive(true);
            //spawnedModels.Add(spawnedModelGO);
        }

        private void AssignModelMaterialReference(int fabricButtonIndex)
        {
            int modelIndex = CurrentModelIndex;
            int catalogueIndex = fabricsManager.CurrentCatalogueIndex;
            Dictionary<string, int> materialReference = new Dictionary<string, int>()
            {
                {"Catalogue", catalogueIndex },
                {"Fabric", fabricButtonIndex }
            };

            if (modelMaterialReferences.ContainsKey(modelIndex))
            {
                modelMaterialReferences[CurrentModelIndex] = materialReference;

            }
            else
            {
                modelMaterialReferences.Add(modelIndex, materialReference);
            }

        }

        #endregion
    }
}

