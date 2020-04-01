using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace helloVoRld.NewScripts.UI
{
    public class Model
    {
        public string modelName;
        public Sprite modelThumbnail;
        public List<string> materialPartNames = new List<string>();
        public int materialIndexToChange;
        public GameObject modelPrefab;
    }
}
