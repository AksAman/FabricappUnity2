using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using helloVoRld.NewScripts.Catalogue;
using helloVoRld.NewScripts.Engine;
using helloVoRld.Networking.RestClient;

namespace helloVoRld.NewScripts.UI
{
    public class CatalogueView : ScrollView<CatalogueButton, CatalogueModel, CatalogueWebModel>
    {
        public override void DownloadList()
        {
            WebClient.Instance.GetCatalogues((_List) =>
            {
                ModelList = _List;
                DownloadingCompleted = true;
            });
        }

        public override void OnUIVisible(object param = null)
        {
            base.OnUIVisible(param);
        }

        public override void OnButtonClick(CatalogueModel button)
        {
            Debug.Log("Button Clicked : " + button.ToString());
        }

    }
}