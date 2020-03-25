using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Lean.Touch;
using UnityEngine.UI;
using System;
using helloVoRld.Test.UI;

namespace helloVoRld.Utilities.CameraUtils
{
    public class CameraZoomer : MonoBehaviour
    {
        #region Variables
        // public
        public float smoothTime = 0.3F;
        public Vector2 minMaxFOV;

        public float thresholdFOVToEnableZoomReset = 30.0f;
        //public SliderTranslator sliderTranslator;
        public Vector2 resetButtonOnOff; // x = on, y = off
        public Button zoomReseter;
        public Ease zoomResetEaseType;
        public float zoomResetDuration;

        // private
        Camera cameraMain;
        float newFOV = 67.0f;
        float originalFOV;
        private float velocity = 0;
        float oldFOV;
        bool isReseterActive = false;
        float zoomResetButtonDuration = 0.1f;
        Ease zoomResetButtonEase = Ease.OutBack;
        #endregion

        void Start()
        {
            cameraMain = GetComponent<Camera>();
            originalFOV = cameraMain.fieldOfView;
            isReseterActive = false;

            FurnitureButton.OnFurnitureButtonClicked += ResetZoom2;
            zoomReseter.onClick.AddListener(() => ResetZoom());
        }

        private void ResetZoom2(int obj)
        {
            ResetZoom();
        }

        private void ResetZoom()
        {
            zoomReseter.GetComponent<RectTransform>().DOAnchorPosY(resetButtonOnOff.y, zoomResetButtonDuration).SetEase(zoomResetButtonEase).OnComplete(() =>
            {
                isReseterActive = false;
                float currentFOV = cameraMain.fieldOfView;
                cameraMain.DOFieldOfView(originalFOV, zoomResetDuration).SetEase(zoomResetEaseType);

            });
            
        }

        protected virtual void OnEnable()
        {
            LeanTouch.OnGesture += HandleGesture;
        }

        protected virtual void OnDisable()
        {
            LeanTouch.OnGesture -= HandleGesture;
        }
        private void HandleGesture(List<LeanFinger> fingers)
        {
            // if finger over UI, do nothing!
            if (fingers[0].IsOverGui) return;

            float pinchRatio = LeanGesture.GetPinchRatio(fingers);
            //Debug.Log("Pinchscale : " + pinchRatio);
            oldFOV = cameraMain.fieldOfView;
            newFOV = Mathf.Clamp(oldFOV * pinchRatio, minMaxFOV.x, minMaxFOV.y);
            cameraMain.fieldOfView = Mathf.SmoothDamp(oldFOV, newFOV, ref velocity, smoothTime);

            // Enable slider based on camera's fov
            if (cameraMain.fieldOfView < thresholdFOVToEnableZoomReset && !isReseterActive)
            {
                zoomReseter.GetComponent<RectTransform>().DOAnchorPosY(resetButtonOnOff.x, zoomResetButtonDuration).SetEase(zoomResetButtonEase);
                isReseterActive = true;
                //zoomReseter.gameObject.SetActive(true);
            }
            else if(cameraMain.fieldOfView > thresholdFOVToEnableZoomReset && isReseterActive)
            {
                zoomReseter.GetComponent<RectTransform>().DOAnchorPosY(resetButtonOnOff.y, zoomResetButtonDuration).SetEase(zoomResetButtonEase);
                isReseterActive = false;
            }
            
        }

        
    }

}
