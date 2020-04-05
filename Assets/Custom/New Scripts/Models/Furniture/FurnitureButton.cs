using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Threading.Tasks;

namespace helloVoRld.NewScripts.Furniture
{
    public class FurnitureButton : ButtonModel<FurnitureModel, FurnitureWebModel>
    {
        [Header("References")]
        public TextMeshProUGUI Name;

        public void Awake()
        {
            var names = new[] { nameof(Name), nameof(Thumbnail), nameof(Button) };
            var objs = new object[] { Name, Thumbnail, Button };

            for (int i = 0; i < names.Length; ++i)
            {
                if (objs[i] == null)
                {
                    throw new ArgumentNullException(names[i], "Watch for reference of object in " + gameObject.name + ".");
                }
            }
        }

        public override void Initialize(FurnitureModel model, Action ButtonClick)
        {
            Name.text = model.Name;

            base.Initialize(model, ButtonClick);
        }

        public override void UnloadOject()
        {
            Name.text = "";

            base.UnloadOject();
        }
    }
}
