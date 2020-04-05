using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using helloVoRld.NewScripts.Fabric;
using helloVoRld.NewScripts.Engine;
using helloVoRld.Networking;

namespace helloVoRld.NewScripts.UI
{
    public class FabricView : ScrollView<FabricButton, FabricModel, FabricWebModel>
    {
        public override void GetList(object param = null)
        {
            int catalogueIndex = (int)param;

            FabricGenerator.Instance.LoadFabrics(catalogueIndex,
                (list) =>
                {
                    ModelList = list;
                    Debug.Log(ModelList.Count);
                    DownloadingCompleted = true;
                },
                () =>
                {
                    Debug.Log("Fabric Loading Failed");
                });
        }

        public override void OnUIVisible(object param = null)
        {
            base.OnUIVisible(param);
        }

        public override void OnButtonClick(FabricModel Model)
        {
            Globals.SelectedFabric = Model;
            MaterialGenerator.Instance.GetAppropriateMaterial(material => 
            {
                // Attach to furniture here
            });
            Debug.Log("Fabric Clicked : " + Model.ToString());
        }

        public override void OnUILeave()
        {
            base.OnUILeave();
        }
    }
}