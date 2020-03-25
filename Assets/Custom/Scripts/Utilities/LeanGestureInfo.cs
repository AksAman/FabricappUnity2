using System;
using System.Collections.Generic;
using Lean.Touch;
using TMPro;
using UnityEngine;

public class LeanGestureInfo : MonoBehaviour
{
    public RectTransform t_fingersCount, t_pinchScale, t_twistRad, t_twistDeg, t_screenDelta;
    int fingersCount;
    float pinchScale, twistRad, twistDeg;
    Vector2 screenDelta;

    void OnEnable()
    {
        LeanTouch.OnGesture += UpdateGestureInfo;
    }

    void OnDisable()
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

    void SetUIText(RectTransform uitextParentRect, string text)
    {
        uitextParentRect.GetChild(1).GetComponent<TextMeshProUGUI>().text = text;
    }
}
