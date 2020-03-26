using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace helloVoRld.Test.Databases
{
    // [CreateAssetMenu(fileName = "New Catalogue", menuName = "Custom/FabricApp/Catalogue")]
    // [System.Serializable]
    public class S_Catalogue// : ScriptableObject
    {
        internal int WEB_Id;
        public string c_name;
        public string c_description;
        public string c_thumbnail_url;
        public string manufacturer_name = "by Marvin Leather";

        public List<Fabric> c_fabrics = new List<Fabric>();

        public override string ToString()
        {
            return c_name + " by " + manufacturer_name;
        }

    }
}