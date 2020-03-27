using helloVoRld.Networking.RestClient;
using helloVoRld.Test.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;
namespace helloVoRld.Test.Databases
{
    // [CreateAssetMenu(fileName = "New Catalogue", menuName = "Custom/FabricApp/Catalogue")]
    // [System.Serializable]
    public class S_Catalogue// : ScriptableObject
    {
        internal int WEB_Id;
        public string c_name;
        public string c_description;
        public string c_thumbnail_url;
        private Sprite c_thumbnail;
        public string manufacturer_name;

        public List<Fabric> c_fabrics = new List<Fabric>();

        private FixedCountDownloader TextureDownloader => CatalogueManager.Instance.TextureDownloader;

        public void LoadThumbnail(Action<Sprite> OnThumbnailDonwloadComplete, Action<float> Progress)
        {
            if (c_thumbnail != null)
            {
                OnThumbnailDonwloadComplete(c_thumbnail);
                return;
            }
            TextureDownloader.AddTask(c_thumbnail_url, (sprite) =>
            {
                c_thumbnail = sprite;
                OnThumbnailDonwloadComplete(sprite);
            },
            (progress) =>
            {
                Progress(progress);
            });
        }

        public override string ToString()
        {
            return c_name + " by " + manufacturer_name;
        }

    }
}