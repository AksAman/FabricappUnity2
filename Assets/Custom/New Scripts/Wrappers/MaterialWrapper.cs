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
			("_MainTex_Tiling", typeof(Vector2)),
			("_Color", typeof(Color)),
			("_MainCon", typeof(float)),
			("_MainBgt", typeof(float)),

			//---- Specular ----
			("_Spec", typeof(float)),

			//----- Roughness -----
			("_RghTex", typeof(Texture2D)),
			("_RghAmp", typeof(float)),

			//----- Normal -----

			//Primary Normal
			("_NormTex", typeof(Texture2D)),
			("_Norm", typeof(float)),
			("_NormBas", typeof(float)),

			//Secondary Normal, and its UV is individually registered
			
			("_UVSec", typeof(float)),
			("_NormTex20", typeof(Texture2D)),
			("_NormTex21", typeof(Texture2D)),

			("_Norm2", typeof(float)),
			("_NormRati", typeof(float)),
			
			("_AOTex", typeof(Texture2D)),
			("_AOStrength", typeof(float)),
		};

		public Material MainObject { get; set; }
		public string Name { get; private set; }


		[JsonProperty]
		public Dictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

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

		public void DeserializeValues(Dictionary<string, object> input)
		{
			MainObject = new Material(Shader.Find("FabricShader"));
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
