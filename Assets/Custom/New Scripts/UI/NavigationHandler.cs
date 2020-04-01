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

            FurnitureButton.onClick.RemoveAllListeners();
            FurnitureButton.onClick.AddListener(() => SwitchToFurniture());
            CatalogueButton.onClick.RemoveAllListeners();
            CatalogueButton.onClick.AddListener(() => SwitchToCatalogues());
        }

        private void SwitchToFurniture()
        {
            SwitchTo(FurniturePanel);
            CatalogueView.Instance.OnUILeave();
        }

        private void SwitchToCatalogues()
        {
            SwitchTo(CataloguePanel);
            CatalogueView.Instance.OnUIVisible();
        }

        internal void SwitchToFabricPanel()
        {
            SwitchTo(FabricPanel);
            CatalogueView.Instance.OnUILeave();
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
