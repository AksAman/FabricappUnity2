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
using helloVoRld.Networking.RestClient;

namespace helloVoRld.NewScripts.UI
{
    public class FurnitureView : ScrollView<FurnitureButton, FurnitureModel, FurnitureWebModel>
    {
        [Header("List Reference")]
        public Test.Databases.S_Models modelsList;

        public override void GetList(object param = null)
        {
            ModelList = new List<FurnitureModel>(from x in modelsList.modelList select new FurnitureModel(x));
            DownloadingCompleted = true;
        }

        public override void OnButtonClick(FurnitureModel Model)
        {
            Debug.Log("Clicked : " + Model.Name);
        }
    }
}