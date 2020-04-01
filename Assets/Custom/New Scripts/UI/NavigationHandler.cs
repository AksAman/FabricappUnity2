using helloVoRld.NewScripts.Engine;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace helloVoRld.NewScripts.UI
{
    public class NavigationHandler : Singleton<NavigationHandler>
    {
        [Header("Buttons")]
        [SerializeField]
        private Button FurnitureButton;
        [SerializeField]
        private Button CatalogueButton;
        [SerializeField]
        private Button BackButton;
        [Header("Panels")]
        [SerializeField]
        private GameObject FurniturePanel;
        [SerializeField]
        private GameObject CataloguePanel;
        [SerializeField]
        private GameObject FabricPanel;

        private GameObject[] Panels { get; set; }

        private void Awake()
        {
            var names = new[] { nameof(FurniturePanel), nameof(CataloguePanel), nameof(FabricPanel) };
            Panels = new[] { FurniturePanel, CataloguePanel, FabricPanel };

            for (int i = 0; i < names.Length; ++i)
            {
                if (Panels[i] == null)
                {
                    throw new ArgumentNullException(names[i], "Watch for reference of object in " + gameObject.name + ".");
                }
            }

            if (FurnitureButton == null)
                throw new ArgumentNullException("Furniture Button", "Watch for reference of object in " + gameObject.name + ".");
            if (CatalogueButton == null)
                throw new ArgumentNullException("Catalogue Button", "Watch for reference of object in " + gameObject.name + ".");
            if (BackButton == null)
                throw new ArgumentNullException("Back Button", "Watch for reference of object in " + gameObject.name + ".");

            if (FurnitureButton.onClick.GetPersistentEventCount() != 0)
                throw new Exception("Remove all Listeners from Furniture Click Button");
            if (CatalogueButton.onClick.GetPersistentEventCount() != 0)
                throw new Exception("Remove all Listeners from Catalogue Click Button");
            if (BackButton.onClick.GetPersistentEventCount() != 0)
                throw new Exception("Remove all Listeners from Back Button");

            FurnitureButton.onClick.RemoveAllListeners();
            FurnitureButton.onClick.AddListener(() => SwitchToFurniture());
            CatalogueButton.onClick.RemoveAllListeners();
            CatalogueButton.onClick.AddListener(() => SwitchToCatalogues());
            BackButton.onClick.RemoveAllListeners();
            BackButton.onClick.AddListener(() => SwitchToCatalogues());
        }

        private void SwitchToFurniture(object param = null)
        {
            FabricView.Instance.OnUILeave();
            CatalogueView.Instance.OnUILeave();
            SwitchTo(FurniturePanel);
        }

        private void SwitchToCatalogues(object param = null)
        {
            FabricView.Instance.OnUILeave();
            CatalogueView.Instance.OnUIVisible(param);
            SwitchTo(CataloguePanel);
        }

        internal void SwitchToFabricPanel(object param = null)
        {
            CatalogueView.Instance.OnUILeave();
            FabricView.Instance.OnUIVisible(param);
            SwitchTo(FabricPanel);
        }

        internal void SwitchTo(GameObject g)
        {
            foreach (var x in Panels)
            {
                x.SetActive(false);
            }

            g.SetActive(true);
        }
    }
}
