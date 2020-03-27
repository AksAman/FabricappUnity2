using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using helloVoRld.Utilities.Debugging;
using helloVoRld.Test.Databases;
using UnityEngine.EventSystems;

namespace helloVoRld.Test.UI
{
    public class FurnitureButton : MonoBehaviour
    {
        #region variables
        [Header("Properties")]
        public int index;

        [Header("References")]
        public TextMeshProUGUI FuB_name;
        public Image FuB_thumbnail;
        public Button FuB_button;

        public static Action<int> OnFurnitureButtonClicked;
        #endregion


        #region main code
        public bool Init(Model model, int index)
        {
            try
            {
                this.index = index;

                if (FuB_name != null) this.FuB_name.text = model.modelName;
                if (FuB_thumbnail != null) this.FuB_thumbnail.sprite = model.modelThumbnail;

                if (FuB_button != null) FuB_button.onClick.AddListener(() => OnClick(this.index));
            }

            catch (Exception exception)
            {
                DebugHelper.LogError(exception.Message);
                return false;
            }

            return true;
        }

        private void OnClick(int index)
        {
            if (OnFurnitureButtonClicked != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(FuB_button.gameObject);
                OnFurnitureButtonClicked(index);
            }
        }
        #endregion

        #region helper code
        #endregion
    }
}

