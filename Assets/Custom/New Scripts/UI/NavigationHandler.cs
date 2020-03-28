using helloVoRld.NewScripts.Engine;
using System;
using UnityEngine;

namespace helloVoRld.NewScripts.UI
{
    public sealed class NavigationHandler : Singleton<NavigationHandler>
    {
        [SerializeField]
        private readonly GameObject FurniturePanel;
        [SerializeField]
        private readonly GameObject CataloguePanel;
        [SerializeField]
        private readonly GameObject FabricPanel;

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
        }

        public void SwitchToFurniture()
        {
            SwitchTo(FurniturePanel);
        }

        public void SwitchToCatalogues()
        {
            SwitchTo(CataloguePanel);
        }

        public void SwitchToFabricPanel()
        {
            SwitchTo(FabricPanel);
        }

        private void SwitchTo(GameObject g)
        {
            foreach (var x in Panels)
            {
                x.SetActive(false);
            }

            g.SetActive(true);
        }
    }
}
