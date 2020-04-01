using System;
using UnityEngine;
using helloVoRld.NewScripts.Engine;
using UnityEngine.UI;

namespace helloVoRld.NewScripts
{
    public abstract class ButtonModel<T, U> : MonoBehaviour
        where T : Model<U>
        where U : IWebModel
    { 
        public Image Thumbnail;
        public Button Button;

        [Header("Loading")]
        public GameObject ProgressObject;
        public Image ProgressBar;

        public virtual void Initialize(T model, Action ButtonClick)
        {
            ProgressObject.SetActive(true);
            model.LoadThumbnail(
                (sprite) =>
                {
                    ProgressObject.SetActive(false);
                    Thumbnail.sprite = sprite;
                },
                (progress) =>
                {
                    ProgressBar.fillAmount = progress;
                });

            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() => ButtonClick());
        }
    }
}