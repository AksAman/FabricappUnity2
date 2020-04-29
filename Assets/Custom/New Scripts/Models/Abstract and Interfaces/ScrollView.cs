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
using helloVoRld.Networking;

namespace helloVoRld.NewScripts
{
    /// <summary>
    /// Manager for ScrollViewer on Screen
    /// </summary>
    /// <typeparam name="T">Button to Pool</typeparam>
    /// <typeparam name="U">Model to implement</typeparam>
    /// <typeparam name="V">JSON model on which model is implemented</typeparam>
    public abstract class ScrollView<T, U, V> : Singleton<ScrollView<T, U, V>>
        where T : ButtonModel<U, V>
        where U : Model<V>
        where V : IWebModel
    {
        /// <summary>
        /// Reference to pass for pooler
        /// </summary>
        [Header("Pooling")]
        public T ObjectToPool;

        /// <summary>
        /// Reference to pass to pooler
        /// </summary>
        [Header("UI Elements")]
        public RectTransform ScrollViewer;
        /// <summary>
        /// Refresh button, to reload ScrollViewer
        /// </summary>
        public Button RefreshButton;

        /// <summary>
        /// Obvious from Name
        /// </summary>
        protected Coroutine UILoaderCoroutine;
        /// <summary>
        /// Pooler, that will be used to pool buttons on current scrollviewer
        /// </summary>
        protected Pooler<T, U, V> PoolObject { get; set; }
        /// <summary>
        /// Texturedownloader, used to get images for thumbnail. Every Button Model refer to this in corresponding cases, for e.g. CatalogueButton
        /// </summary>
        internal FixedCountDownloader TextureDownloader { get; set; }
        /// <summary>
        /// Processed list from JSON that keep record of all data
        /// </summary>
        public List<U> ModelList { get; protected set; }

        /// <summary>
        /// Used to delay Pooler to pool buttons, before the models are loaded. Must be set true only in GetList() method
        /// </summary>
        protected bool DownloadingCompleted = false;
        /// <summary>
        /// Parameter which is passed to current scrollview. Currently used for FabricScrollView only since the buttons depend on catalogue clicked
        /// </summary>
        private object ObjectPassed { get; set; } = null;

        /// <summary>
        /// Constructor
        /// </summary>
        public void Awake()
        {
            TextureDownloader = new FixedCountDownloader(this);
            PoolObject = new Pooler<T, U, V>(ObjectToPool, ScrollViewer);
            if (RefreshButton != null)
            {
                RefreshButton.onClick.RemoveAllListeners();
                RefreshButton.onClick.AddListener(() => 
                {
                    OnUILeave(); 
                    OnRefreshClickAction(ObjectPassed); 
                    OnUIVisible(ObjectPassed); 
                });
            }
        }

        /// <summary>
        /// Just to call TexturwDownloader next frame processing
        /// </summary>
        public void Update()
        {
            TextureDownloader.Update();
        }

        /// <summary>
        /// Method which will be called to populate ModelList
        /// </summary>
        /// <param name="param">Parameter passed to this scrollviewer</param>
        public abstract void GetList(object param = null);

        /// <summary>
        /// Called from navigation handler or Refresh, when we are set to display this scrollviewer on screen
        /// </summary>
        /// <param name="param">Parameter passed to this scrollviewer</param>
        public virtual void OnUIVisible(object param = null)
        {
            ObjectPassed = param;
            DownloadingCompleted = false;
            GetList(param);
            UILoaderCoroutine = StartCoroutine(LoadUI());
        }

        /// <summary>
        /// Process when we have List of models on hand to be applied to buttons
        /// </summary>
        /// <returns>Enumerator</returns>
        IEnumerator LoadUI()
        {
            while (!DownloadingCompleted)
                yield return null;

            // Maps Model to Buttons
            PoolObject.FillViewer(ModelList.Count, (index, Button) =>
            {
                Button.GetComponent<T>().Initialize(ModelList[index], () =>
                {
                    OnButtonClick(ModelList[index]);
                });
            });

            UILoaderCoroutine = null;
        }

        /// <summary>
        /// Callback when button is clicked
        /// </summary>
        /// <param name="Model">Model of button which was clicked</param>
        public abstract void OnButtonClick(U Model);

        /// <summary>
        /// Called from Navigation Handler or Refresh when we dispose all objects of current scrollview
        /// </summary>
        public virtual void OnUILeave()
        {
            // Remove Thumbanil of all buttons
            foreach (var x in PoolObject.AllObjects)
                x.Thumbnail.sprite = null;

            // If Early UILeave before even UI Load completed
            if (UILoaderCoroutine != null)
            {
                StopCoroutine(UILoaderCoroutine);
                UILoaderCoroutine = null;
            }

            // Pool disponsing of buttons
            PoolObject.ClearViewer();
        }

        /// <summary>
        /// Work required when Refresh is clicked. Will be called after OnUILeave is called and before next OnUIVisible
        /// </summary>
        /// <param name="param">Parameter that was originally passed to OnUIVisible</param>
        public virtual void OnRefreshClickAction(object param = null)
        {

        }
    }
}