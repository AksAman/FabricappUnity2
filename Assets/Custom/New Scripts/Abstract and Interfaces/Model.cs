using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using helloVoRld.Networking.RestClient;

namespace helloVoRld.NewScripts
{
    public abstract class Model<T> where T : IWebModel
    {
        public readonly string ThumbnailURL;
        protected Sprite ThumbnailSprite;

        public bool IsThumbnailLoaded => ThumbnailSprite == null;

        protected abstract FixedCountDownloader TextureDownloader { get; }

        protected Model(string URL)
        {
            ThumbnailURL = (Globals.IP + "/" + URL).Replace("//", "/");
            ThumbnailSprite = null;
        }

        public void LoadThumbnail(Action<Sprite> OnSuccess, Action<float> Progress)
        {
            if (ThumbnailSprite != null)
            {
                OnSuccess(ThumbnailSprite);
                return;
            }

            TextureDownloader.AddTask(ThumbnailURL,
                (sprite) =>
                {
                    ThumbnailSprite = sprite;
                    OnSuccess(ThumbnailSprite);
                },
                (progress) => 
                {
                    Progress(progress);
                });
        }
    }
}