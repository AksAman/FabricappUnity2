using helloVoRld.Utilities.Debugging;
using System.Collections;
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
        public List<AllowedParts> allowedParts = new List<AllowedParts>();
        public int meshForMaterialUpdate = 0;
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

    [System.Serializable]
    public struct AllowedParts
    {
        public string name;
        public bool allowed;
    }
}

