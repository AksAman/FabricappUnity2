using helloVoRld.Test.Databases;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using helloVoRld.Utilities.Debugging;
using UnityEngine.EventSystems;

namespace helloVoRld.Test.UI
{
    public class CatalogueButton : MonoBehaviour
    {
        #region variables
        [Header("Properties")]
        public int index;
        //public S_Catalogue catalogue;

        [Header("References")]
        public TextMeshProUGUI CB_name;
        public TextMeshProUGUI CB_description;
        public TextMeshProUGUI CB_manufacturer;
        public Image CB_thumbnail;
        public Button CB_button;

        public static Action<int> OnCatalogueButtonClicked;
        #endregion


        #region main code
        public bool Init(S_Catalogue catalogue, int index)
        {
            try
            {
                this.index = index;

                if (CB_name != null) this.CB_name.text = catalogue.c_name;
                //if (CB_description != null) this.CB_description.text = catalogue.c_description;
                if (CB_manufacturer != null) this.CB_manufacturer.text = catalogue.manufacturer_name;
                if (CB_thumbnail != null) this.CB_thumbnail.sprite = catalogue.c_thumbnail;

                if (CB_button != null) CB_button.onClick.AddListener(() => OnClick(this.index));
            }

            catch (Exception exception)
            {
                DebugHelper.Log(exception.Message);
                return false;
            }

            return true;
        }

        private void OnClick(int index)
        {
            if(OnCatalogueButtonClicked !=null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(CB_button.gameObject);
                OnCatalogueButtonClicked(index);
            }
        }
        #endregion

        #region helper code
        #endregion
    }
}

