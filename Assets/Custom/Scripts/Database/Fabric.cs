using UnityEngine;
using System;
using helloVoRld.Networking.RestClient;

namespace helloVoRld.Test.Databases
{
    [System.Serializable]
    public class Fabric
    {
        public string f_title;
        public string f_thumbnail_url;
        Sprite f_thumbnail;
        //public Texture f_diffuseTexture;
        public Material f_material;

        FixedCountDownloader TextureDownloader;

        public void LoadThumbnail(Action<Sprite> OnThumbnailDonwloadComplete, Action<float> Progress)
        {
            return;
            if (f_thumbnail != null)
            {
                OnThumbnailDonwloadComplete(f_thumbnail);
                return;
            }
            TextureDownloader.AddTask(f_thumbnail_url,
                (sprite) =>
                {
                    f_thumbnail = sprite;
                    OnThumbnailDonwloadComplete(sprite);
                },
                (progress) =>
                {
                    Progress(progress);
                });
            
        }
    }
}