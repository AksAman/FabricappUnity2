using helloVoRld.NewScripts.Catalogue;
using helloVoRld.NewScripts.Furniture;
using helloVoRld.NewScripts.Fabric;
using System.Collections.Generic;
using helloVoRld.Networking.Models;
using Newtonsoft.Json;
using UnityEngine;
using helloVoRld.NewScripts.Wrappers;
using System;
using System.IO;

namespace helloVoRld
{
    public static class Globals
    {
        public static readonly string IP = @"http://hvrplfabricapp.ml";
        public static readonly string AllCataloguesIP = IP + @"/fabricapp/api/catalogues";
        public static readonly string AllFabricsIP = IP + @"/fabricapp/api/fabrics";
        public static readonly RequestHeader RequestHeader = new RequestHeader { Key = "Authorization", Value = "Token " + "0ef442a637f1570b5f848f164ee972219eaca8bc" };

        public static string ThumbnailsFolderLocation = Application.persistentDataPath + "/Data/Thumbnails/";
        public static string TexturesFolderLocation = Application.persistentDataPath + "/Data/Textures/";

        public static readonly List<CatalogueModel> Catalogues = new List<CatalogueModel>();
        public static readonly List<FurnitureModel> Furnitures = new List<FurnitureModel>();

        public static FurnitureModel SelectedFurniture;
        public static CatalogueModel SelectedCatalogue;
        public static FabricModel SelectedFabric;

        static Globals()
        {
            Catalogues.Clear();
            Furnitures.Clear();
        }

        public static string FabricIP(int catIndex)
        {
            return IP + @"/fabricapp/api/fabrics?catpk=" + catIndex;
        }

        public static string MaterialIP(int index)
        {
            return IP + "/fabricapp/api/materials?pk=" + index;
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

        public static bool IsThumbnailOnDisk(string ThumbnailURL, string Date, out Sprite sp)
        {
            sp = null;
            if (!Directory.Exists(ThumbnailsFolderLocation))
                return false;

            var temp = ThumbnailURL.Split(new char[] { '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length < 2)    // For Filename and extension, just for corner case
                return false;

            string fileloc = ThumbnailsFolderLocation + temp[temp.Length - 2].Replace("%20", " ") + " - " + Date + "." + temp[temp.Length - 1];

            if (File.Exists(fileloc))
            {
                Texture2D tex = new Texture2D(2, 2);
                tex.LoadImage(File.ReadAllBytes(fileloc));
                sp = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                return true;
            }

            return false;
        }

        public static void WriteThumbnailOnDisk(string ThumbnailURL, string Date, Sprite sp)
        {
            if (!Directory.Exists(ThumbnailsFolderLocation))
                Directory.CreateDirectory(ThumbnailsFolderLocation);

            if (sp == null)
                return;

            var temp = ThumbnailURL.Split(new char[] { '/', '.' }, StringSplitOptions.RemoveEmptyEntries);

            if (temp.Length < 2)    // For Filename and extension, just for corner case
                return;

            string fileloc = ThumbnailsFolderLocation + temp[temp.Length - 2].Replace("%20", " ") + " - " + Date + "." + temp[temp.Length - 1];
            File.WriteAllBytes(fileloc, sp.texture.EncodeToPNG());
        }

        public static bool IsTextureOnDisk(string TextureURL, string Date, out Texture2D tex)
        {
            tex = null;
            if (!Directory.Exists(TexturesFolderLocation))
                return false;

            var temp = TextureURL.Split(new char[] { '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length < 2)    // For Filename and extension, just for corner case
                return false;
        
            string fileloc = TexturesFolderLocation + temp[temp.Length - 2].Replace("%20", " ") + " - " + Date + "." + temp[temp.Length - 1];
            
            if (File.Exists(fileloc))
            {
                tex = new Texture2D(2, 2);
                tex.LoadImage(File.ReadAllBytes(fileloc));
                return true;
            }

            return false;
        }

        public static void WriteTextureOnDisk(string TextureURL, string Date, Texture2D tex)
        {
            if (!Directory.Exists(TexturesFolderLocation))
                Directory.CreateDirectory(TexturesFolderLocation);

            if (tex == null)
                return;

            var temp = TextureURL.Split(new char[] { '/', '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (temp.Length < 2)    // For Filename and extension, just for corner case
                return;

            string fileloc = TexturesFolderLocation + temp[temp.Length - 2].Replace("%20", " ") + " - " + Date + "." + temp[temp.Length - 1];
            File.WriteAllBytes(fileloc, tex.EncodeToPNG());
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
            if (type == typeof(Vector2))
            {
                Vector2 v = material.GetTextureScale(name.Substring(0, name.IndexOf("_Tiling")));
                return string.Format("{0},{1}", v.x, v.y);
            }

            throw new Exception();
        }

        [Obsolete]
        public static void SetPropertyString(this Material m, string name, Type type, object value)
        {
            if (type == typeof(Vector2))
            {
                string[] temp = (value as string).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (temp.Length == 0)
                    m.SetTextureScale(name.Substring(0, name.LastIndexOf("_Tiling")),
                        new Vector2(1, 1));
                else
                    m.SetTextureScale(name.Substring(0, name.LastIndexOf("_Tiling")),
                        new Vector2(float.Parse(temp[0]), float.Parse(temp[1])));
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
                m.SetTexture(name, null);/*
                // Null was exported
                if (val == null || val.Length == 0)
                    m.SetTexture(name, null);
                else
                    m.SetTexture(name, null/* AssetDatabase.LoadAssetAtPath<Texture2D>(val));*/
            }
        }
    }
}
