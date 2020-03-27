﻿using helloVoRld.Test.Databases;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using helloVoRld.Utilities.Debugging;
using UnityEngine.EventSystems;
using helloVoRld.Networking.RestClient;

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
        bool TextureLoaded = false;
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
                if (!TextureLoaded && CB_thumbnail != null)
                {
                    FixedCountDownloader.Instance.AddTask(catalogue.c_thumbnail_url, (sprite) => 
                    {
                        CB_thumbnail.sprite = sprite;
                        TextureLoaded = true;
                    },
                    (progress) =>
                    {
                         Debug.Log((progress * 100).ToString("0.00"));
                    });
                }
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

