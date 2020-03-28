using System;
using TMPro;
using UnityEngine;

namespace helloVoRld.NewScripts.Catalogue
{
    public class CatalogueButton : ButtonModel<CatalogueModel, CatalogueWebModel>
    {
        [Header("References")]
        public TextMeshProUGUI Name;
        public TextMeshProUGUI Description;
        public TextMeshProUGUI Manufacturer;

        private void Awake()
        {
            var names = new[] { nameof(Name), nameof(Description), nameof(Manufacturer), nameof(Thumbnail), nameof(Button), nameof(ProgressObject), nameof(ProgressBar) };
            var objs = new object[] { Name, Description, Manufacturer, Thumbnail, Button, ProgressObject, ProgressBar };

            for (int i = 0; i < names.Length; ++i)
            {
                if (objs[i] == null)
                {
                    throw new ArgumentNullException(names[i], "Watch for reference of object in " + gameObject.name + ".");
                }
            }
        }

        public void Initialize(CatalogueModel model, int index)
        {
            Name.text = model.Name;
            Description.text = model.Description;
            Manufacturer.text = model.ManufacturerName;

            base.Initialize(model, () => CatalogueManager.Instance.OnModelButtonClick(index));
        }
    }
}