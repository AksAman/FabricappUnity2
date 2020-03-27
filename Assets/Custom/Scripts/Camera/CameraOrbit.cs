using DG.Tweening;
using Lean.Touch;
using UnityEngine;

namespace helloVoRld.Utilities.CameraUtils
{
    public class CameraOrbit : MonoBehaviour
    {
        private enum CurrentState
        {
            NoTouch,
            JustTouched,
            Dragging
        }

        protected Transform _XForm_Camera;
        protected Transform _XForm_Parent;
        protected Vector3 _LocalRotation;
        public Quaternion cameraParentDefaultRotation;

        public Vector2 camMinMaxRot;
        public float MouseSensitivity = 4f;
        public float OrbitDampening = 10f;


        private CurrentState state = CurrentState.NoTouch;
        private bool isResetingCamera;

        //public ScriptableBool overui;
        //public Tweener tweener;

        private readonly bool isScrolling;

        // Use this for initialization
        private void Start()
        {
            _XForm_Camera = transform;
            _XForm_Parent = transform.parent;
            cameraParentDefaultRotation = _XForm_Parent.rotation;
            //FurnitureButton.OnFurnitureButtonClicked += ResetRotation;
            isResetingCamera = false;
        }

        private void LateUpdate()
        {

            if (Input.GetMouseButtonDown(0))
            {
                state = CurrentState.JustTouched;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                state = CurrentState.NoTouch;
            }

            bool isSingleFinger = LeanTouch.Fingers.Count == 1;
            if (isSingleFinger)
            {
                //_LocalRotation = Vector3.zero;
                if (LeanTouch.Fingers[0].IsOverGui)
                {
                    return;
                }

                if (state == CurrentState.JustTouched && Input.GetMouseButtonDown(0))
                {
                    state = CurrentState.Dragging;
                    return;
                }
                if (state == CurrentState.Dragging)
                {
                    _LocalRotation.x += Input.GetAxis("Mouse X") * MouseSensitivity;
                    _LocalRotation.y -= Input.GetAxis("Mouse Y") * MouseSensitivity;

                    // Clamp the y Rotation to horizon and not flipping over at the top
                    if (_LocalRotation.y < camMinMaxRot.x)
                    {
                        _LocalRotation.y = camMinMaxRot.x;
                    }
                    else if (_LocalRotation.y > camMinMaxRot.y)
                    {
                        _LocalRotation.y = camMinMaxRot.y;
                    }
                }


            }
            //if (!isResetingCamera)
            {
                Quaternion QT = Quaternion.Euler(_LocalRotation.y, _LocalRotation.x, 0);
                _XForm_Parent.rotation = Quaternion.Lerp(_XForm_Parent.rotation, QT, Time.deltaTime * OrbitDampening);

            }

        }

        #region Helper

        private void ResetRotation(int furnitureButtonIndex)
        {
            isResetingCamera = true;
            _XForm_Parent.DORotateQuaternion(cameraParentDefaultRotation, 0.5f).SetEase(Ease.InSine).OnComplete(() => isResetingCamera = false);
        }


        #endregion
    }
}
