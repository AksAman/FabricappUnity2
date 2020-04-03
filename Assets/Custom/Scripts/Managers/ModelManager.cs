/*using helloVoRld.Core.Singletons;
using System.Collections.Generic;
using UnityEngine;
using helloVoRld.Test.Databases;
using helloVoRld.Core.Pooling;
using helloVoRld.Utilities;
using helloVoRld.Test.UI;
using helloVoRld.Utilities.Debugging;
using TMPro;
using helloVoRld.Utilities.Outliner;

namespace helloVoRld.Test.Managers
{
    [RequireComponent(typeof(ObjectPooler))]
    public class ModelManager : Singleton<ModelManager>
    {
        #region Variables
        // public
        [Header("Database")]
        public S_Models s_models;

        // Fields
        public GameObject CurrentModel { get => currentModel; private set => currentModel = value; }
       
        [SerializeField] S_CurrentModel s_currentModel;

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
            if(furnitureButtonPooler.isPoolInitialized)
            {
                PopulateFurnitureButtons();
            }

            FurnitureButton.OnFurnitureButtonClicked += FurnitureButtonClicked;

            if(s_models.modelList.Count > 0)
            {
                ChangeFurniture(0);
            }

            if(rememberMaterials)
            {
                FabricButton.OnFabricButtonClicked += AssignModelMaterialReference;
            }
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
            currentFurnitureText.text = $"Furniture : {s_models.modelList[furnitureButtonIndex].modelName}".ToUpper();
        }

        private void PopulateFurnitureButtons()
        {
            TransformUtils.ClearChilds(furnitureScrollContentHolder, furnitureButtonPooler);
            loadingProgress = 0;
            int modelsCount = s_models.modelList.Count;
            if (modelsCount > 0)
            {
                for (int i = 0; i < modelsCount; i++)
                {
                    //Instantiate
                    GameObject catalogueButtonGO = furnitureButtonPooler.GetObject();
                    catalogueButtonGO.transform.SetParent(furnitureScrollContentHolder);
                    catalogueButtonGO.GetComponent<RectTransform>().localScale = Vector3.one;
                    //Initialize
                    bool success = catalogueButtonGO.GetComponentInChildren<FurnitureButton>().Init(s_models.modelList[i], i);

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
            loadingProgress = ((float)(indexLoaded + 1) / s_models.modelList.Count) * 100;
        }

        private void ChangeFurniture(int newIndex)
        {
            
            if(CurrentModel)
            {
                if (newIndex == s_currentModel.currentModelIndex) return;
                Destroy(CurrentModel);
            }

            //CurrentModelIndex = newIndex;
            s_currentModel.currentModelIndex = newIndex;
            UpdateCurrentFurnitureText(s_currentModel.currentModelIndex);

            GameObject spawnedModelGO = Instantiate(s_models.modelList[s_currentModel.currentModelIndex].modelPrefab, modelHolder);
            
            CurrentModel = spawnedModelGO;
            s_currentModel.currentSubModelIndex = s_models.modelList[s_currentModel.currentModelIndex].meshForMaterialUpdate;

            if(rememberMaterials)
            {
                //Apply last remembered material
                if (modelMaterialReferences.ContainsKey(s_currentModel.currentModelIndex))
                {
                    int catalogueIndex = modelMaterialReferences[s_currentModel.currentModelIndex]["Catalogue"];
                    int fabricIndex = modelMaterialReferences[s_currentModel.currentModelIndex]["Fabric"];
                    materialManager.ChangeCurModelMaterial(s_models.modelList[s_currentModel.currentModelIndex].materialIndexToChange, catalogueIndex, fabricIndex);
                }

            }
            else
            {
                int catalogueIndex = fabricsManager.CurrentCatalogueIndex;
                int fabricIndex = materialManager.CurrentFabricIndex;
                materialManager.ChangeCurModelMaterial(s_models.modelList[s_currentModel.currentModelIndex].materialIndexToChange, catalogueIndex, fabricIndex);
            }

            // Set gameobject active
            spawnedModelGO.SetActive(true);
            //spawnedModels.Add(spawnedModelGO);
        }

        private void AssignModelMaterialReference(int fabricButtonIndex)
        {
            int modelIndex = s_currentModel.currentModelIndex;
            int catalogueIndex = fabricsManager.CurrentCatalogueIndex;
            Dictionary<string, int> materialReference = new Dictionary<string, int>()
            {
                {"Catalogue", catalogueIndex },
                {"Fabric", fabricButtonIndex }
            };

            if (modelMaterialReferences.ContainsKey(modelIndex))
            {
                modelMaterialReferences[s_currentModel.currentModelIndex] = materialReference;

            }
            else
            {
                modelMaterialReferences.Add(modelIndex, materialReference);
            }

        }

        #endregion
    }
}

*/