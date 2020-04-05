using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;
using helloVoRld.NewScripts.Engine;

namespace helloVoRld.NewScripts.Wrappers
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ColorWrapper : IWrapper<Color>
    {
        #region NonExportData
        /// <summary>
        /// Object from interface
        /// </summary>
        public Color MainObject { get; set; }
        #endregion

        #region ExportData
        [JsonProperty]
        int R;
        [JsonProperty]
        int G;
        [JsonProperty]
        int B;
        [JsonProperty]
        int H;
        [JsonProperty]
        int S;
        [JsonProperty]
        int V;

        [JsonProperty]
        string RGBHEX;
        [JsonProperty]
        string HSVHEX;

        #endregion

        /// <summary>
        /// Default Constructor, required for JsonDeserializer
        /// </summary>
        private ColorWrapper() { }
        public ColorWrapper(Color c)
        {
            SetProperties(c);
        }

        public void SetProperties(Color c)
        {
            MainObject = c;
            Color.RGBToHSV(c, out var h, out var s, out var v);

            R = (int)(c.r * 255);
            G = (int)(c.g * 255);
            B = (int)(c.b * 255);
            H = (int)(h * 255);
            S = (int)(s * 255);
            V = (int)(v * 255);

            RGBHEX = c.ToHexString();
            HSVHEX = new Color(h, s, v).ToHexString();
        }

        /// <summary>
        /// Will be called to recreate all objects when JsonDeserializer will complete it's task
        /// </summary>
        /// <param name="context"></param>
        [OnDeserialized]
        void OnDeserializedMethod(StreamingContext context)
        {
            MainObject = new Color(((float)R) / 255, ((float)G) / 255, ((float)B) / 255);
        }
    }
}
