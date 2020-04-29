using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace helloVoRld.NewScripts.Catalogue
{
    /// <summary>
    /// Script that must be attached to Catalogue Button Prefab
    /// </summary>
    public class CatalogueButton : ButtonModel<CatalogueModel, CatalogueWebModel>
    {
        [Header("References")]
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Description;
        public TextMeshProUGUI Manufacturer;

        private void Awake()
        {
            // Check for All References
            var names = new[] { nameof(Name),  nameof(Manufacturer), nameof(Thumbnail), nameof(Button), nameof(ProgressObject), nameof(ProgressBar) };
            var objs = new object[] { Name,  Manufacturer, Thumbnail, Button, ProgressObject, ProgressBar };

            for (int i = 0; i < names.Length; ++i)
            {
                if (objs[i] == null)
                {
                    throw new ArgumentNullException(names[i], "Watch for reference of object in " + gameObject.name + ".");
                }
            }
        }

        public override void Initialize(CatalogueModel model, Action OnButtonClick)
        {
            Name.text = model.Name;
            Manufacturer.text = model.ManufacturerName;

            base.Initialize(model, OnButtonClick);
        }

        public override void UnloadOject()
        {
            Name.text = "";
            Manufacturer.text = "";

            base.UnloadOject();
        }
    }
}