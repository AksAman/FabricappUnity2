/*using UnityEngine;
using System;
using helloVoRld.Networking;
using helloVoRld.Test.Managers;

namespace helloVoRld.Test.Databases
{
    [System.Serializable]
    public class Fabric
    {
        public string f_title;
        public string f_thumbnail_url;
        //public Texture f_diffuseTexture;
        public Material f_material;

        FixedCountDownloader TextureDownloader => FabricsManager.Instance.TextureDownloader;

        public void LoadThumbnail(Action<Sprite> OnThumbnailDonwloadComplete, Action<float> Progress)
        {
            TextureDownloader.AddTask(f_thumbnail_url,
                (sprite) =>
                {
                    OnThumbnailDonwloadComplete(sprite);
                },
                (progress) =>
                {
                    Progress(progress);
                });
            
        }
    }
}*/