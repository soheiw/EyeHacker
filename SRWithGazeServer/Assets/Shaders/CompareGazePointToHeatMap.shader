Shader "Unlit/CompareGazePointToHeatMap"
{
	Properties
	{
		_MainTex("HeatMap", 2D) = "white" {}
		_OverTex("GazePoint", 2D) = "white" {}
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
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
            sampler2D _OverTex;
			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 over = tex2D(_OverTex, i.uv);

				// apply fog
				// UNITY_APPLY_FOG(i.fogCoord, col);
                fixed heatMap = (col.r + col.g + col.b) / 3.0;
                fixed gaze = (over.r + over.g + over.b) / 3.0;
                if(heatMap > 0.1 && gaze > 0.1){
                    col.r = 1.0;
                    col.g = 0.0;
                    col.b = 0.0;
                }

				return col;
			}
			ENDCG
		}
	}
}