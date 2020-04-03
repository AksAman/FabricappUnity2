using helloVoRld.NewScripts.Catalogue;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using helloVoRld.NewScripts.Wrappers;
using System;

namespace helloVoRld
{
    public static class Globals
    {
        public static readonly string IP = @"http://hvrplfabricapp.ml";
        public static readonly string AllCataloguesIP = IP + @"/fabricapp/api/catalogues";
        public static readonly string AllFabricsIP = IP + @"/fabricapp/api/fabrics";
        public static readonly List<CatalogueModel> Catalogues = new List<CatalogueModel>();

        public static string FabricIP(int catIndex)
        {
            return IP + @"/fabricapp/api/fabrics?catpk=" + catIndex;
        }


        public static string ToHexString(this Color c, bool IncludeA = false)
        {
            int R = (int)(c.r * 255);
            int G = (int)(c.g * 255);
            int B = (int)(c.b * 255);
            int A = (int)(c.a * 255);

            // int.ToString("X2") converts the int to two digit Hex
            if (!IncludeA)
                return string.Format(@"0x{0}{1}{2}", R.ToString("X2"), G.ToString("X2"), B.ToString("X2"));
            else
                return string.Format(@"0x{0}{1}{2}{3}", R.ToString("X2"), G.ToString("X2"), B.ToString("X2"), A.ToString("X2"));
        }

        [Obsolete]
        public static object GetPropertyString(this Material material, string name, Type type)
        {
            if (type == typeof(float))
                return material.GetFloat(name).ToString();
            if (type == typeof(Color))
                return material.GetColor(name).ToHexString(IncludeA: true);
            if (type == typeof(Texture2D))
                return null;// AssetDatabase.GetAssetPath(material.GetTexture(name));
            if (type == typeof(Vector2Wrapper))
                return new Vector2Wrapper(material.GetTextureScale(name.Substring(0, name.IndexOf("_Tiling"))));

            throw new Exception();
        }

        [Obsolete]
        public static void SetPropertyString(this Material m, string name, Type type, object value)
        {
            if (type == typeof(Vector2Wrapper))
            {
                Vector2Wrapper wrapper = JsonConvert.DeserializeObject<Vector2Wrapper>(value.ToString());
                m.SetTextureScale(name.Substring(0, name.LastIndexOf("_Tiling")), wrapper.MainObject);
                return;
            }

            string val = value as string;

            if (type == typeof(float))
                m.SetFloat(name, float.Parse(val));
            if (type == typeof(Color))
            {
                float r = int.Parse(val.Substring(2, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
                float g = int.Parse(val.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
                float b = int.Parse(val.Substring(6, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
                float a = 1;
                if (val.Length == 10)
                    a = int.Parse(val.Substring(8, 2), System.Globalization.NumberStyles.HexNumber) / 255f;

                m.SetColor(name, new Color(r, g, b, a));
            }

            if (type == typeof(Texture2D))
            {
                // Null was exported
                if (val.Length == 0)
                    m.SetTexture(name, null);
                else
                    m.SetTexture(name, null/* AssetDatabase.LoadAssetAtPath<Texture2D>(val)*/);
            }
        }
    }
}
