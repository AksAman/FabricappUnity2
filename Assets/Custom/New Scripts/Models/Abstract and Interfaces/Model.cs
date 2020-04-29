using helloVoRld.Networking;
using System;
using UnityEngine;

namespace helloVoRld.NewScripts
{
    /// <summary>
    /// Data Model, that stores the useful data extracted from JSON
    /// </summary>
    /// <typeparam name="T">Webmodel on which this class is implemented</typeparam>
    public abstract class Model<T> where T : IWebModel
    {
        /// <summary>
        /// URL of Thumbnail to display on button
        /// </summary>
        public readonly string ThumbnailURL;
        /// <summary>
        /// Thumbnail to display on button
        /// </summary>
        protected Sprite ThumbnailSprite;
        /// <summary>
        /// Modified date, that was writen with data in JSON
        /// </summary>
        public readonly string DateSuffix;

        /// <summary>
        /// Check for whether thumbnail was previously loaded in current running app instance
        /// </summary>
        public bool IsThumbnailLoaded => ThumbnailSprite == null;

        /// <summary>
        /// Reference to download manager in corresponding Button Model
        /// </summary>
        protected abstract FixedCountDownloader TextureDownloader { get; }

        /// <summary>
        /// Base Constructor
        /// </summary>
        /// <param name="URL">JSON URL to thumbnail of button</param>
        /// <param name="Date">Last modified date of thumbnail</param>
        protected Model(string URL, string Date)
        {
            ThumbnailURL = Globals.IP + URL;
            ThumbnailSprite = null;
            DateSuffix = string.Join("_", Date.Split(new char[] { '-', ' ', ':' }));
        }

        /// <summary>
        /// Called to get the thumbnial for button
        /// </summary>
        /// <param name="OnSuccess">Action to perform when program gets the thumbnail</param>
        /// <param name="Progress">Callback to get track of progress during download from Web</param>
        public void LoadThumbnail(Action<Sprite> OnSuccess, Action<float> Progress)
        {
            // Thumbnail already loaded previously
            if (ThumbnailSprite != null)
            {
                OnSuccess(ThumbnailSprite);
                return;
            }

            //  Thumbnail already available on disk
            if (Globals.IsThumbnailOnDisk(ThumbnailURL, DateSuffix, out Sprite sp))
            {
                ThumbnailSprite = sp;
                OnSuccess(ThumbnailSprite);
                return;
            }

            // Get thumbnail from Net
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
        }
    }
}