using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using helloVoRld.NewScripts.Engine;

namespace helloVoRld.NewScripts.UI
{
    public sealed class NavigationHandler : Singleton<NavigationHandler>
    {
        [SerializeField]
        private GameObject FurniturePanel;
        [SerializeField]
        private GameObject CataloguePanel;
        [SerializeField]
        private GameObject FabricPanel;

        private GameObject[] Panels { get; set; }

        void Awake()
        {
            var names = new[] { nameof(FurniturePanel), nameof(CataloguePanel), nameof(FabricPanel) };
            Panels = new[] { FurniturePanel, CataloguePanel, FabricPanel };

            for (int i = 0; i < names.Length; ++i)
                if (Panels[i] == null)
                    throw new ArgumentNullException(names[i], "Watch for reference of object in " + gameObject.name + ".");
        }

        public void SwitchToFurniture() => SwitchTo(FurniturePanel);
        public void SwitchToCatalogues() => SwitchTo(CataloguePanel);
        public void SwitchToFabricPanel() => SwitchTo(FabricPanel);

        private void SwitchTo(GameObject g)
        {
            foreach (var x in Panels)
                x.SetActive(false);

            g.SetActive(true);
        }
    }
}
