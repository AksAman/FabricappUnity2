using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Michsky.UI.ModernUIPack;

namespace helloVoRld.Test.UI
{
    public class ColorSwapButton : MonoBehaviour
    {
        public static Action<UIGradient> OnWallButtonClicked;
        private Button thisButton;
        private UIGradient thisGradient;

        private void Start()
        {
            thisButton = GetComponent<Button>();
            thisGradient = GetComponentInChildren<UIGradient>();
            thisButton.onClick.AddListener(() => WallButtonClicked(thisGradient));
        }

        private void WallButtonClicked(UIGradient thisGradient)
        {
            if (OnWallButtonClicked != null)
            {
                OnWallButtonClicked(thisGradient);
            }
        }
    }
}

