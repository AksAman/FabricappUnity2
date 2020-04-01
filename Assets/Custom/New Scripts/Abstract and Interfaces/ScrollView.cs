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

namespace helloVoRld.NewScripts
{
    public abstract class ScrollView<T, U, V> : Singleton<ScrollView<T, U, V>>
        where T : ButtonModel<U, V>
        where U : Model<V>
        where V : IWebModel
    {
        [Header("Pooling")]
        public T ObjectToPool;

        [Header("UI Elements")]
        public RectTransform ScrollViewer;

        protected Coroutine DownloaderCoroutine;
        protected Pooler<T, U, V> PoolObject { get; set; }
        internal FixedCountDownloader TextureDownloader { get; set; }
        public List<U> ModelList { get; protected set; } = new List<U>();
        protected bool DownloadingCompleted = false;

        public void Awake()
        {
            TextureDownloader = new FixedCountDownloader(this);
            PoolObject = new Pooler<T, U, V>(ObjectToPool, ScrollViewer);
        }

        public void Update()
        {
            TextureDownloader.Update();
        }

        public abstract void GetList(object param = null);

        public virtual void OnUIVisible(object param = null)
        {
            DownloadingCompleted = false;
            GetList(param);
            DownloaderCoroutine = StartCoroutine(LoadUI());
        }

        IEnumerator LoadUI()
        {
            while (!DownloadingCompleted)
                yield return null;

            PoolObject.FillViewer(ModelList.Count, (index, Button) =>
            {
                Button.GetComponent<T>().Initialize(ModelList[index], () => OnButtonClick(ModelList[index]));
            });

            DownloaderCoroutine = null;
        }

        public abstract void OnButtonClick(U Model);

        public virtual void OnUILeave()
        {
            foreach (var x in PoolObject.AllObjects)
                x.Thumbnail.sprite = null;
            if (DownloaderCoroutine != null)
            {
                StopCoroutine(DownloaderCoroutine);
                DownloaderCoroutine = null;
                PoolObject.ClearViewer();
            }
        }
    }
}