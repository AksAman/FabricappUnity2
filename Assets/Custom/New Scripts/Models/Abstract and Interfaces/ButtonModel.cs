using System;
using UnityEngine;
using helloVoRld.NewScripts.Engine;
using UnityEngine.UI;

namespace helloVoRld.NewScripts
{
    /// <summary>
    /// Base class of button that will be displayed on scrollviewer. Child inheriting from this class must be attached to Button prefab
    /// </summary>
    /// <typeparam name="T">Model that implements the data to display</typeparam>
    /// <typeparam name="U">Model that was used to deserialize JSON</typeparam>
    public abstract class ButtonModel<T, U> : MonoBehaviour
        where T : Model<U>
        where U : IWebModel
    {
        /// <summary>
        /// Image Reference for Button onto which thumbnail stored in model will be displayed
        /// </summary>
        public Image Thumbnail;
        /// <summary>
        /// Self reference, used to implement onclick methods
        /// </summary>
        public Button Button;

        /// <summary>
        /// Parent of Progress bar
        /// </summary>
        [Header("Loading")]
        public GameObject ProgressObject;
        /// <summary>
        /// Progress Bar, 0-100%
        /// </summary>
        public Image ProgressBar;

        /// <summary>
        /// Must be called when button is instantiated from pooler and we need to set values to be displayed on button
        /// </summary>
        /// <param name="model">Data model, that will be implemented on this button</param>
        /// <param name="ButtonClick">Action that will be called when button on screen will be clicked</param>
        public virtual void Initialize(T model, Action ButtonClick)
        {
            // Null Check
            if (ProgressObject != null)
                ProgressObject.SetActive(true);

            // Start loading thumbnail from here
            model.LoadThumbnail(
                (sprite) =>
                {
                    if (ProgressObject != null)
                        ProgressObject.SetActive(false);
                    Thumbnail.sprite = sprite;
                },
                (progress) =>
                {
                    if (ProgressBar != null)
                        ProgressBar.fillAmount = progress;
                });

            // Attack listeners to button
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() => ButtonClick());
        }

        /// <summary>
        /// Must be called form Pooler only, when button is not required anymore on current display
        /// </summary>
        public virtual void UnloadOject()
        {
            Thumbnail.sprite = null;
            Button.onClick.RemoveAllListeners();
        }
    }
}