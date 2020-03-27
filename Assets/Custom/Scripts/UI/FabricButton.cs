using helloVoRld.Test.Databases;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using helloVoRld.Utilities.Debugging;
using UnityEngine.EventSystems;
using helloVoRld.Networking.RestClient;
namespace helloVoRld.Test.UI
{
    public class FabricButton : MonoBehaviour
    {
        #region variables
        [Header("Properties")]
        public int index;

        [Header("References")]
        public TextMeshProUGUI FB_name;
        public Image FB_thumbnail;
        public Button FB_button;

        public GameObject progressElements;
        public Image progressBar;

        public static Action<int> OnFabricButtonClicked;
        #endregion


        #region main code
        public bool Init(Fabric fabric, int index)
        {
            try
            {
                this.index = index;

                if (FB_name != null) this.FB_name.text = fabric.f_title;
                if (FB_thumbnail != null)
                {
                    FB_thumbnail.sprite = null;
                    fabric.LoadThumbnail(
                        (sprite) =>
                        {
                            ToggleProgressElements(false);
                            FB_thumbnail.sprite = sprite;
                        },
                        (progress) =>
                        {
                            ToggleProgressElements(true);
                            progressBar.fillAmount = progress;
                        });
                }
                if (FB_button != null) FB_button.onClick.AddListener(() => OnClick(this.index));
            }

            catch (Exception exception)
            {
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(FB_button.gameObject);
                DebugHelper.Log(exception.Message);
                return false;
            }

            return true;
        }

        private void OnClick(int index)
        {
            if (OnFabricButtonClicked != null)
            {
                OnFabricButtonClicked(index);
            }
        }
        #endregion

        #region helper code
        private void ToggleProgressElements(bool newState)
        {
            progressElements.SetActive(newState);
        }

        #endregion
    }
}

