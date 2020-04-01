﻿using System;
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
        protected Pooler CataloguePooler { get; set; }
        internal FixedCountDownloader TextureDownloader { get; set; }
        protected List<U> ModelList { get; set; } = new List<U>();
        protected bool DownloadingCompleted = false;

        public void Awake()
        {
            TextureDownloader = new FixedCountDownloader(this);
            CataloguePooler = new Pooler(ObjectToPool.gameObject, ScrollViewer);
            if (!DownloadingCompleted)
                DownloadList();
        }

        public void Update()
        {
            TextureDownloader.Update();
        }

        public abstract void DownloadList();

        public virtual void OnUIVisible(object param = null)
        {
            DownloaderCoroutine = StartCoroutine(LoadUI());
        }

        IEnumerator LoadUI()
        {
            while (!DownloadingCompleted)
                yield return null;

            CataloguePooler.FillViewer(ModelList.Count, (index, Button) =>
            {
                Button.GetComponent<T>().Initialize(ModelList[index], () => OnButtonClick(ModelList[index]));
            });

            DownloaderCoroutine = null;
        }

        public abstract void OnButtonClick(U VAL);

        public void OnUILeave()
        {
            if (DownloaderCoroutine != null)
            {
                StopCoroutine(DownloaderCoroutine);
                DownloaderCoroutine = null;
                CataloguePooler.ClearViewer();
            }
        }
    }
}