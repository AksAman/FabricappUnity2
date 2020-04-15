using helloVoRld.Networking;
using System;
using UnityEngine;

namespace helloVoRld.NewScripts
{
    public abstract class Model<T> where T : IWebModel
    {
        public readonly string ThumbnailURL;
        protected Sprite ThumbnailSprite;
        public readonly string DateSuffix;

        public bool IsThumbnailLoaded => ThumbnailSprite == null;

        protected abstract FixedCountDownloader TextureDownloader { get; }

        protected Model(string URL, string Date)
        {
            ThumbnailURL = Globals.IP + URL;
            ThumbnailSprite = null;
            DateSuffix = string.Join("_", Date.Split(new char[] { '-', ' ', ':' }));
        }

        public void LoadThumbnail(Action<Sprite> OnSuccess, Action<float> Progress)
        {
            if (ThumbnailSprite != null)
            {
                OnSuccess(ThumbnailSprite);
                return;
            }

            GlobalMonobehaviour.Instance.StartCoroutine(
                Globals.IsThumbnailOnDisk(ThumbnailURL, DateSuffix,
                (sp) =>
                {
                    ThumbnailSprite = sp;
                    OnSuccess(ThumbnailSprite);
                    return;
                },
                () =>
                {
                    TextureDownloader.AddTask(ThumbnailURL,
                        (sprite) =>
                        {
                            ThumbnailSprite = sprite;
                            OnSuccess(ThumbnailSprite);
                            Globals.WriteThumbnailOnDisk(ThumbnailURL, DateSuffix, sprite);
                        },
                        (progress) =>
                        {
                            Progress(progress);
                        });
                }));
        }
    }
}