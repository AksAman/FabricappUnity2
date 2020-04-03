using helloVoRld.Utilities.Debugging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace helloVoRld.Test.Databases
{
    [CreateAssetMenu(fileName = "New Model Database", menuName = "Custom/FabricApp/Models")]
    [System.Serializable]
    public class S_Models : ScriptableObject
    {
        public List<Model> modelList;
    }
}
