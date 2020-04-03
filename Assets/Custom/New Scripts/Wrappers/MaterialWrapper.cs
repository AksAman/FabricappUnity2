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
	public class MaterialWrapper : IWrapper<Material>
	{
		private static readonly List<(string PropName, Type PropType)> PropertiesNames = new List<(string PropName, Type PropType)>
		{
            //----- Albedo and Texture Coordinates  ----
            ("_MainTex", typeof(Texture2D)),
			("_MainTex_Tiling", typeof(Vector2Wrapper)),
			("_Rotation", typeof(float)),
			("_Color", typeof(Color)),
			("_MainCon", typeof(float)),
			("_MainBgt", typeof(float)),

			//---- Dirt Pattern ----

			//Dirt UV is individually registered
			("_DirtTex", typeof(Texture2D)),
			("_DirtTex_Tiling", typeof(Vector2Wrapper)),
			("_DirtGray", typeof(float)),
			("_Dirt1", typeof(float)),
			("_Dirt0", typeof(float)),

			//Macro Adjustment
			("_DirtPre", typeof(float)),
			("_DirtCol", typeof(Color)),
			("_DirtRgh", typeof(float)),
			("_DirtNorm", typeof(float)),

			//---- Specular ----
			("_Spec", typeof(float)),

			//----- Roughness -----
			("_RghTex", typeof(Texture2D)),
			("_RghTex_Tiling", typeof(Vector2Wrapper)),
			("_RghGray", typeof(float)),
			("_Rgh1", typeof(float)),
			("_RghAmp", typeof(float)),


			//----- Normal -----

			//Primary Normal
			("_NormTex", typeof(Texture2D)),
			("_NormTex_Tiling", typeof(Vector2Wrapper)),
			("_Norm", typeof(float)),
			("_NormBas", typeof(float)),

			//Secondary Normal, and its UV is individually registered
			("_NormTex2", typeof(Texture2D)),
			("_Norm2", typeof(float)),
			("_NormRati", typeof(float))
		};

		public Material MainObject { get; set; }
		public string Name { get; private set; }


		[JsonProperty]
		internal Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

		public MaterialWrapper() { }

		public MaterialWrapper(Material m)
		{
			SetProperties(m);
		}

		public void SetProperties(Material m)
		{
			MainObject = m;

			string path = AssetDatabase.GetAssetPath(m);
			Name = path.Substring(path.LastIndexOf("/") + 1);
			Properties.Clear();
			Properties.Add("Name", Name);

			foreach (var (name, type) in PropertiesNames)
			{
				Properties.Add(name, m.GetPropertyString(name, type));
			}
		}

		internal void DeserializeValues(Dictionary<string, object> input)
		{
			MainObject = new Material(Shader.Find("Standard"));
			Properties = input;

			for (int i = 0; i < PropertiesNames.Count; ++i)
			{
				MainObject.SetPropertyString(PropertiesNames[i].PropName, PropertiesNames[i].PropType, Properties[PropertiesNames[i].PropName]);
			}
		}


		[OnDeserialized]
		void OnDeserializedMethod(StreamingContext context)
		{
			DeserializeValues(Properties);
		}
	}
}
