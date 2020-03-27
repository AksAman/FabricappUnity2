using helloVoRld.Utilities.Debugging;
using System.Collections.Generic;
using UnityEngine;

namespace helloVoRld.Test.Databases
{
    [System.Serializable]
    public class Model
    {
        #region Variables
        public string modelName;
        public Sprite modelThumbnail;
        public List<string> materialPartNames = new List<string>();
        public int materialIndexToChange;
        public GameObject modelPrefab;
        #endregion


        #region Main functions
        #endregion


        #region Helper Functions
        public GameObject GetModelMesh()
        {
            GameObject modelMesh = null;
            try
            {
                modelMesh = modelPrefab.transform.GetChild(0).gameObject;
            }
            catch (System.Exception exc)
            {
                DebugHelper.LogException(exc);
                //throw;
            }
            return modelMesh;

        }
        #endregion
    }
}

