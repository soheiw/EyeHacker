Shader "Unlit/AlphaOverlap"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_OverTex("MaskingTexture", 2D) = "white" {}
        [Toggle(SET_ALPHA_ZERO)]
        _SetAlphaZero("Set Alpha Zero", Float) = 0
        _AdjustAlpha("Adjust Alpha", Range(0,1)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			// #pragma multi_compile_fog

            #pragma shader_feature SET_ALPHA_ZERO
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				// UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _OverTex;

			float4 _MainTex_ST;
            fixed _AdjustAlpha;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				// o.uv.x = 1.0 - o.uv.x;

				// UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
                
				fixed4 over = tex2D(_OverTex, i.uv);
                
                #ifdef SET_ALPHA_ZERO
                    over.a = 0.0;
                #else
                    // use original alpha
                #endif

                fixed alphaRatio = over.a * _AdjustAlpha;
				col.r = col.r * (1.0 - alphaRatio) + over.r * alphaRatio;
				col.g = col.g * (1.0 - alphaRatio) + over.g * alphaRatio;
				col.b = col.b * (1.0 - alphaRatio) + over.b * alphaRatio;
				
				return col;
			}
			ENDCG
		}
	}
}
