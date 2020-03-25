using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using UnityEngine.UI;
using helloVoRld.Networking.RestClient;
using helloVoRld.Core.Singletons;
using helloVoRld.Utilities.Debugging;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace helloVoRld.Test.Managers
{

    public class SlideshowManager : Singleton<SlideshowManager>
    {
        #region Variables
        [SerializeField] private List<Sprite> slideShowImages = new List<Sprite>();
        [SerializeField] private int debugImageCount = 10;

        [SerializeField] private int secondsToWaitForSlideshow = 50;
        [SerializeField] private float fadeDuration = 0.3f;
        [SerializeField] private WaitForSeconds waitAfterFadeIn = new WaitForSeconds(2);
        [SerializeField] private bool isFingerDown;
        [SerializeField] private bool isSlideShowActive;
        [SerializeField] private float timer = 0;
        [SerializeField] private float loadingProgress;

        [SerializeField] private GameObject slideShowPanel;
        [SerializeField] private Image slideShowImage;
        [SerializeField] private Image slideShowMask;
        [SerializeField] private CanvasGroup slideshowCanvasGroup;


        #endregion

        #region MainFunction
        protected virtual void OnEnable()
        {
            LeanTouch.OnFingerDown += HandleFingerDown;
            LeanTouch.OnFingerUp += HandleFingerUp;
        }

        protected virtual void OnDisable()
        {
            LeanTouch.OnFingerDown -= HandleFingerDown;
            LeanTouch.OnFingerUp -= HandleFingerUp;
        }

        private void Start()
        {
            LoadImages();    
        }

        private void Update()
        {
            if(isFingerDown)
            {
                timer = 0;
                StopSlideShow();
            }
            else if(!isFingerDown & !isSlideShowActive)
            {
                timer += Time.deltaTime;
                if(timer > secondsToWaitForSlideshow)
                {
                    StartSlideShow();
                }
            }
        }

        #endregion

        #region HelperFunctions
        private void LoadImages()
        {
            
            for (int i = 0; i < debugImageCount; i++)
            {
                string width = "1280";
                string height = "800";
                string url = $"https://picsum.photos/{width}/{height}";
                StartCoroutine(RestWebClient.Instance.HttpDownloadImage(url, SetImageInList, i));
            }
        }

        private void HandleFingerDown(LeanFinger finger)
        {
            isFingerDown = true;
        }

        private void HandleFingerUp(LeanFinger finger)
        {
            isFingerDown = false;
            
        }

        private void StartSlideShow()
        {
            isSlideShowActive = true;
            if(slideShowImages.Count > 0)
            {
                slideshowCanvasGroup.DOFade(1.0f, fadeDuration).OnComplete(() =>
                {
                    //slideShowImage.raycastTarget = true;
                    slideShowPanel.SetActive(isSlideShowActive);
                    StartCoroutine(SlideShow());
                });
            }
            
            
        }

        private void StopSlideShow()
        {
            isSlideShowActive = false;
            StopCoroutine("SlideShow");
            slideshowCanvasGroup.DOFade(0.0f, fadeDuration).OnComplete(() => {
                slideShowPanel.SetActive(isSlideShowActive);
                //slideShowImage.raycastTarget = false;
            });
        }

        private void SetLoadProgress()
        {
            loadingProgress = ((float)(slideShowImages.Count) / debugImageCount) * 100;
        }

        private void SetImageInList(ImageResponse response, int index)
        {
            Texture2D downloadedTexture = response.textureDownloaded;
            if (string.IsNullOrEmpty(response.Error) && downloadedTexture!=null)
            {
                Sprite createdSprite = Sprite.Create(response.textureDownloaded, new Rect(0.0f, 0.0f, downloadedTexture.width, downloadedTexture.height), new Vector2(0.5f, 0.5f));
                slideShowImages.Add(createdSprite);
                SetLoadProgress();
            }
            else
            {
                DebugHelper.LogError(response.Error);
            }
        }

        private IEnumerator SlideShow()
        {
            int imageIndex = 0;
            slideShowImage.sprite = null;
            while (isSlideShowActive)
            {
                //yield return new WaitForSeconds(fadeDuration);
                slideShowImage.DOFade(0.0f, fadeDuration);
                yield return new WaitForSeconds(fadeDuration);
                slideShowImage.sprite = slideShowImages[imageIndex];
                slideShowImage.DOFade(1.0f, fadeDuration);
                //yield return new WaitForSeconds(fadeDuration);
                yield return waitAfterFadeIn;
                imageIndex = (imageIndex + 1) % slideShowImages.Count;

            }
        }
        #endregion
    }
}
