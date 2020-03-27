using helloVoRld.Utilities.Debugging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace helloVoRld.Test.Managers
{
    public class GameManager : MonoBehaviour
    {
        #region Variables
        #endregion


        #region Main functions

        private void Start()
        {
            Application.targetFrameRate = 60;
            Application.quitting += Application_quitting;
            Application.focusChanged += Application_focusChanged;
            Application.lowMemory += Application_lowMemory;
            
        }

        private void Application_lowMemory()
        {
            DebugHelper.Log("Low Memory");
        }

        private void Application_focusChanged(bool newfocus)
        {
            DebugHelper.Log("Focus : "+ newfocus.ToString());
        }

        private void Application_quitting()
        {
            DebugHelper.Log("Quitting");
        }
        #endregion


        #region Helper Functions
        #endregion
    }
}

