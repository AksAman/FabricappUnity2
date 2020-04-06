using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using helloVoRld.NewScripts.Engine;
using System.Runtime.Serialization;
using UnityEngine;
using Newtonsoft.Json;
/*
namespace helloVoRld.NewScripts.Wrappers
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Vector2Wrapper : IWrapper<Vector2>
    {
        public Vector2 MainObject { get; set; }

        [JsonProperty]
        public float X { get; set; }
        [JsonProperty]
        public float Y { get; set; }

        private Vector2Wrapper() { }

        public Vector2Wrapper(Vector2 v) => SetProperties(v);

        public void SetProperties(Vector2 t)
        {
            MainObject = t;
            X = t.x;
            Y = t.y;
        }

        [OnDeserialized]
        void OnDeserializedMethod(StreamingContext context)
        {

        }

    }
}
*/