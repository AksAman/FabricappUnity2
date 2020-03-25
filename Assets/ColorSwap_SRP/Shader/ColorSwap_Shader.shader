Shader "Unlit/ColorSwap_Shader"
{
    Properties
    {
		_Radius("radius", Float) = 0.800000
		_Offset("Offset", Vector) = (0.000000,0.000000,0.000000,0.000000)
		_BackColor("BackColor", Color) = (1.000000,0.976415,0.976415,0.000000)
		_FrontColor("FrontColor", Color) = (1.000000,0.231132,0.231132,0.000000)
    }
    SubShader
    {
        Tags { "Queue" = "Background" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float2 screenPos:TEXCOORD1;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
				float4 screenPos:TEXCOORD1;
            };

			float _Radius;
			float4 _Offset;
			fixed4 _BackColor;
			fixed4 _FrontColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
                return o;
            }

			float Circle(float2 v)
			{
				return sqrt(v.x * v.x + v.y * v.y);
			}

			fixed4 frag(v2f IN) : COLOR
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
				//return float4(circle_step,0.0,0.0 ,1.0);

				return lerp(_BackColor, _FrontColor, circle_step);
				//return worldpos;
			}
            ENDCG
        }
    }
}
