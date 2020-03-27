using helloVoRld.Core.Singletons;
using helloVoRld.Test.UI;
using TMPro;
using UnityEngine;
namespace helloVoRld.Test.Managers
{
    public class MaterialManager : Singleton<MaterialManager>
    {
        #region Variables
        // fields
        public int CurrentFabricIndex { get => currentFabricIndex; private set => currentFabricIndex = value; }

        // private
        [SerializeField] private readonly TextMeshProUGUI currentFabricText;
        private ModelManager modelManager;
        private CatalogueManager catalogueManager;
        private FabricsManager fabricsManager;
        private int currentFabricIndex;
        #endregion


        #region Main functions

        private void Start()
        {
            modelManager = ModelManager.Instance;
            catalogueManager = CatalogueManager.Instance;
            fabricsManager = FabricsManager.Instance;
            FabricButton.OnFabricButtonClicked += FabricButtonClicked;
        }

        #endregion

        #region Helper Functions
        private void FabricButtonClicked(int fabricButtonIndex)
        {
            CurrentFabricIndex = fabricButtonIndex;
            int catalogueIndex = fabricsManager.CurrentCatalogueIndex;
            //UpdateCurrentFabricText(CurrentFabricIndex, catalogueIndex);
            ChangeCurModelMaterial(modelManager.models[modelManager.CurrentModelIndex].materialIndexToChange,
                catalogueIndex, CurrentFabricIndex);
        }


        public void ChangeCurModelMaterial(int atIndex, Material newMaterial)
        {
            GameObject modelMesh = modelManager.CurrentModel.transform.GetChild(0).gameObject;
            if (modelMesh)
            {
                Material[] allMats = modelMesh.GetComponent<MeshRenderer>().materials;
                allMats[atIndex] = newMaterial;
                modelMesh.GetComponent<MeshRenderer>().materials = allMats;

            }
        }
        public void ChangeCurModelMaterial(int atIndex, int currentCatalogueIndex, int currentFabricIndex)
        {
            UpdateCurrentFabricText(currentCatalogueIndex, currentFabricIndex);
            GameObject modelMesh = modelManager.CurrentModel.transform.GetChild(0).gameObject;
            if (modelMesh)
            {
                Material[] allMats = modelMesh.GetComponent<MeshRenderer>().materials;
                allMats[atIndex] = catalogueManager.Catalogues[currentCatalogueIndex].c_fabrics[currentFabricIndex].f_material;
                modelMesh.GetComponent<MeshRenderer>().materials = allMats;
                //DebugHelper.Log("material changed");

            }
        }

        private void UpdateCurrentFabricText(int currentCatalogueIndex, int currentFabricIndex)
        {
            var current_catalogue = catalogueManager.Catalogues[currentCatalogueIndex];
            currentFabricText.text = $"Fabric : {current_catalogue.c_name} {current_catalogue.c_fabrics[currentFabricIndex].f_title}".ToUpper();
        }
        #endregion
    }
}

