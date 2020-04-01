using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace helloVoRld.NewScripts.Fabric
{
    public class FabricButton : ButtonModel<FabricModel, FabricWebModel>
    {
        [Header("References")]
        public TextMeshProUGUI Name;

        private void Awake()
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

        public override void Initialize(FabricModel model, Action ButtonClick)
        {
            Name.text = model.Title;
            base.Initialize(model, ButtonClick);
        }
    }
}
