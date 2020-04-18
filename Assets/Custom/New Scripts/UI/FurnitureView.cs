using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using helloVoRld.NewScripts.Furniture;
using helloVoRld.NewScripts.Engine;
using helloVoRld.Networking;
using TMPro;

namespace helloVoRld.NewScripts.UI
{
    public class FurnitureView : ScrollView<FurnitureButton, FurnitureModel, FurnitureWebModel>
    {
        [Header("List Reference")]
        public Test.Databases.S_Models modelsList;

        [Header("Additional References")]
        public TMP_Text FooterText;

        public void Start()
        {
            OnUIVisible();
        }

        public override void GetList(object param = null)
        {
            ModelList = new List<FurnitureModel>(from x in modelsList.modelList select new FurnitureModel(x));
            Globals.Furnitures.Clear();
            Globals.Furnitures.AddRange(ModelList);
            DownloadingCompleted = true;
            Globals.SelectedFurniture = ModelList[0];
        }

        public override void OnButtonClick(FurnitureModel Model)
        {
            Globals.SelectedFurniture = Model;
            Debug.Log("Clicked : " + Model.ToString());
            FooterText.text = $"FURNITURE : { Model.Name }";
        }
    }
}