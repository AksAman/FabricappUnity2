Shader "FabricShader" {
	
	Properties {
		
		//----- Albedo and Texture Coordinates  ----
		
		_MainTex ("Albedo: Map", 2D) = "white" {}
		_Color ("Albedo: Color Bias", Color) = (1,1,1,1)
		_MainCon ("Albedo: Contrast", Range(0,4)) = 1.0
		_MainBgt ("Albedo: Brightness", Range(0,4)) = 1.0


		_Spec ("Specular: Uniform Strength", Range(-1,1)) = 0.0

		//----- Roughness -----

		[NoScaleOffset] _RghTex ("Roughness: Map", 2D) = "black" {}
		_RghAmp ("Smoothness: Strength", Range(0,1)) = 1.0

		//----- Normal -----

		//Primary Normal
		[NoScaleOffset] _NormTex ("Normal: Map", 2D) = "bump" {}
		_Norm ("Normal: Strength", Range(-2,2)) = 1.0
		_NormBas ("Normal: Bias", Range(-1, 1)) = 0.0
		//Secondary Normal, and its UV is individually registered

		[Enum(UV0,0,UV1,1)] _UVSec("UV Set for secondary textures", Float) = 0
		[NoScaleOffset] _NormTex20  ("Normal: Secondary Map", 2D) = "bump" {}
		[NoScaleOffset] _NormTex21("Normal: Secondary Map", 2D) = "bump" {}

		_Norm2 ("Normal: Secondary Strength", Range(-2,4)) = 0.0
		_NormRati ("Normal: Secondary to Primary Bias Ratio", Range(0,2)) = 0.0

		[NoScaleOffset]	_AOTex("Ambient Occlusion: Map", 2D) = "white" {}
		_AOStrength("Ambient Occlusion: Strength", Range(0,4)) = 1.0

		//---- Additions ----

	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 300

		CGPROGRAM

		#pragma surface surf StandardSpecular addshadow fullforwardshadows vertex:vert
		#pragma target 3.0
		//#include "UnityCG.cginc"
		//---- Texture Varibles ----

		sampler2D _MainTex;
		sampler2D _AOTex;

		sampler2D _RghTex;
		sampler2D _NormTex;
		sampler2D _NormTex20;
		sampler2D _NormTex21;


		//---- Varibles ----

		//Albedo 
		fixed4 _Color;
		half _MainCon;
		half _MainBgt;
		half _AOStrength;

		//Specular
		half _Spec;
		//Roughness
		half _RghAmp;
		//Normal
		half _UVSec;

		half _Norm;
		half _NormBas;
		half _Norm2;
		half _NormRati;


		struct Input {
			float4 _MainTex_ST;
			half2 uv_MainTex;
			half2 uv_RghTex;
			half2 uv_NormTex20;

			half2 uv2_NormTex21;
		
		};


		void vert (inout appdata_full v) {

        }

		void surf (Input IN, inout SurfaceOutputStandardSpecular o) 
		{
			//Base Color Assignment
			fixed3 c0 = tex2D (_MainTex, IN.uv_MainTex);
			//fixed3 ao = tex2D(_AOTex, IN.uv_MainTex);
			fixed3 ao;
			if (_UVSec == 0)
			{
				ao = tex2D(_AOTex, IN.uv_NormTex20);
			}
			else
			{
				ao = tex2D(_AOTex, IN.uv2_NormTex21);
			}
			fixed3 aoApplied = c0 * pow(ao, _AOStrength);

			fixed3 c1 = pow (aoApplied, _MainCon);
			fixed3 c2 = c1 * _Color.rgb;
			fixed3 c3 = c2 * _MainBgt;
			
			o.Albedo = c3;
			o.Specular = _Spec;
			half r0 = tex2D(_RghTex, IN.uv_MainTex).r;

			o.Smoothness = r0* _RghAmp;


			//Primary Evaluation
			half3 n10 = UnpackNormal (tex2D (_NormTex, IN.uv_MainTex));
			half3 n11 = lerp (n10, half3 (0,0,1), (1 - _Norm));
			half3 n20;
			//Secondary Evaluation 
			if (_UVSec == 0)
			{
				n20 = UnpackNormal(tex2D(_NormTex20 , IN.uv_NormTex20));
			}
			else
			{
				n20 = UnpackNormal(tex2D(_NormTex21, IN.uv2_NormTex21));

			}
			//half3 n20 = UnpackNormal(tex2D(_NormTex20 , IN.uv_NormTex2));
			half3 n21 = lerp (n20, half3 (0,0,1), (1 - _Norm2));
			//Evantual Evaluation 
			half3 n1 = n11 + _NormRati * n21 + _NormBas * half3 (0,0,1);
			//Applying Dirt Pattern to Normal
			//half3 n0 = lerp (n1 + _DirtNorm * half3 (0,0,1), n1, dd);
			//Finalized Normal
			half3 n =  normalize (n1);

			o.Normal =  n;

		}
		ENDCG
	}
	FallBack "Diffuse"
}