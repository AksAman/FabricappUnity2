using Lean.Touch;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeanGestureInfo : MonoBehaviour
{
    public RectTransform t_fingersCount, t_pinchScale, t_twistRad, t_twistDeg, t_screenDelta;
    private int fingersCount;
    private float pinchScale, twistRad, twistDeg;
    private Vector2 screenDelta;

    private void OnEnable()
    {
        LeanTouch.OnGesture += UpdateGestureInfo;
    }

    private void OnDisable()
    {
        LeanTouch.OnGesture -= UpdateGestureInfo;
    }

    private void UpdateGestureInfo(List<LeanFinger> fingers)
    {
        fingersCount = fingers.Count;
        pinchScale = LeanGesture.GetPinchScale(fingers);
        twistRad = LeanGesture.GetTwistRadians(fingers);
        twistDeg = LeanGesture.GetTwistDegrees(fingers);
        screenDelta = LeanGesture.GetScreenDelta(fingers);

        SetUIText(t_fingersCount, fingersCount.ToString());
        SetUIText(t_pinchScale, pinchScale.ToString());
        SetUIText(t_twistRad, twistRad.ToString());
        SetUIText(t_twistDeg, twistDeg.ToString());
        SetUIText(t_screenDelta, screenDelta.ToString());
    }

    private void SetUIText(RectTransform uitextParentRect, string text)
    {
        uitextParentRect.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
    }
}
