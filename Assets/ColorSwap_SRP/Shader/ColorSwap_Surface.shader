Shader "Custom/ColorSwap_Surface"
{
    Properties
    {
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0

		_Radius("radius", Float) = 0.800000
		_Offset("Offset", Vector) = (0.000000,0.000000,0.000000,0.000000)
		_BackColor("BackColor", Color) = (1,1,1,1)
		_FrontColor("FrontColor", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        //#pragma surface surf Standard fullforwardshadows
		#pragma surface surf Lambert noforwardadd
        // Use shader model 3.0 target, to get nicer looking lighting
        //#pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
			float4 screenPos;
            float2 uv_MainTex;
			
        };

        half _Glossiness;
        half _Metallic;

		float _Radius;
		float4 _Offset;
		fixed4 _BackColor;
		fixed4 _FrontColor;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

		float Circle(float2 v)
		{
			return sqrt(v.x * v.x + v.y * v.y);
		}

        void surf (Input IN, inout SurfaceOutput o)
        {
			//screencoords
			float4 worldpos = float4(IN.screenPos.xy / IN.screenPos.w, 0.0, 1.0);
			// offset
			worldpos.xy += _Offset.xy;
			// aspect correction
			float aspect_ratio = _ScreenParams.y / _ScreenParams.x;
			worldpos.y *= aspect_ratio;
			// make circle
			float circle = Circle(worldpos.xy);
			float circle_step = step(circle, _Radius);
			float4 circleCol = lerp(_BackColor, _FrontColor, circle_step);

			//return lerp(_BackColor, _FrontColor, circle_step);
			//return worldpos;

            // Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * circleCol;
            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            /*o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;*/
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
