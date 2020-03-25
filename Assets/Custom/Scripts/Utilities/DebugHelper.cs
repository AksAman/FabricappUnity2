using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace helloVoRld.Utilities.Debugging
{
    public static class DebugHelper
    {
        public static void Log(string message)
        {
            Debug.Log(message);
            ShowAndroidToastMessage(message); 
        }

        public static void LogWarning(string message)
        {
            Debug.LogWarning(message);
            ShowAndroidToastMessage(message);
        }

        public static void LogError(string message)
        {
            Debug.LogError(message);
            ShowAndroidToastMessage(message);
        }

        public static void LogException(Exception exception)
        {
            Debug.LogException(exception);
            ShowAndroidToastMessage(exception.Message);
        }


        private static void ShowAndroidToastMessage(string message)
        {
            try
            {
                AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                if (unityPlayer != null)
                {
                    AndroidJavaObject unityActivity =
                    unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

                    if (unityActivity != null)
                    {
                        AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                        unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                        {
                            AndroidJavaObject toastObject =
                                toastClass.CallStatic<AndroidJavaObject>(
                                    "makeText", unityActivity, message, 0);
                            toastObject.Call("show");
                        }));
                    }
                }
            }
            catch (Exception exc)
            {
                //Debug.Log(exc.Message);
                ////throw;
            }
        }
    }
}

