using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEngine;
using helloVoRld.NewScripts.Engine;

namespace helloVoRld.NewScripts.Wrappers
{
    [JsonObject(MemberSerialization.OptIn)]
    public class TextureWrapper : IWrapper<Texture2D>
    {
        #region NonExportData
        internal static double Fraction = 0.2;

        public Texture2D MainObject { get; set; }

        private Color[] ColorArray { get; set; }
        private int Width => (int)(MainObject.width * Fraction);
        private int Height => (int)(MainObject.height * Fraction);

        #endregion

        #region ExportData

        [JsonProperty]
        public string Name { get; private set; }

        [JsonProperty]
        public ColorWrapper MaximumOccuringColor { get; internal set; }

        [JsonProperty]
        public ColorWrapper AverageColor { get; internal set; }
        #endregion

        public TextureWrapper() { }

        public TextureWrapper(Texture2D texture)
        {
            SetProperties(texture);
        }

        public void SetProperties(Texture2D texture)
        {
            MainObject = texture;

            string path = AssetDatabase.GetAssetPath(texture);
            Name = path.Substring(path.LastIndexOf("/") + 1);

            ColorArray = new Color[Width * Height];

            for (int i = 0; i < Height; ++i)
            {
                for (int j = 0; j < Width; ++j)
                {
                    // Linear map from 2 Dimensions to color array
                    ColorArray[Width * i + j] = MainObject.GetPixel(i, j);
                }
            }
        }

        public Color GetAverageColor()
        {
            return (AverageColor = new ColorWrapper(new Color(ColorArray.Average(a => a.r),
                ColorArray.Average(a => a.g),
                ColorArray.Average(a => a.b),
                ColorArray.Average(a => a.a)))).MainObject;
        }

        public Color GetMaximumOccuringColor()
        {
            // TValue (int) : Count of occurences of TKey (color)
            Dictionary<Color, int> dict = new Dictionary<Color, int>();
            foreach (var c in ColorArray)
            {
                if (dict.ContainsKey(c))
                {
                    dict[c]++;
                }
                else
                {
                    dict.Add(c, 1);
                }
            }

            return (MaximumOccuringColor = new ColorWrapper(
                dict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key)
                ).MainObject;
        }

        [OnSerialized]
        void OnDeserializedMethod(StreamingContext context)
        {

        }
    }
}
