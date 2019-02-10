Shader "Unlit/GazeColoredHeatMap"
{
	Properties
	{
		_MainTex("HeatMap", 2D) = "white" {}
		_GazeU("GazeU", Float) = 1
        _GazeV("GazeV", Float) = 1
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
			float4 _MainTex_ST;

            float _GazeU;
            float _GazeV;
			
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

                /* col.r = i.uv.x > (_GazeU - 6) ? 1 : 0; 
                col.r = i.uv.x > (_GazeU + 6) ? 0 : 1; 
                col.r = i.uv.y > (_GazeV - 6) ? 1 : 0; 
                col.r = i.uv.y > (_GazeV + 6) ? 0 : 1; */
                
                if(i.uv.x == (int)_GazeU && i.uv.y == (int)_GazeV)
                {
                    col.r = 1.0;
                    col.g = 0.0;
                    col.b = 0.0;
                }

				// apply fog
				// UNITY_APPLY_FOG(i.fogCoord, col);
                
                /*for (int x = _GazeU - 6; x <= _GazeU + 6; x++)
                {
                    for (int y = _GazeV - 6; y <= _GazeV + 6; y++)
                    {
                        if (x >= 0 && x <= 159 && y >= 0 && y <= 89)
                        {
                            col.r = 1.0;
                            col.g = 0.0;
                            col.b = 0.0;
                        }
                        else
                        {
                            // not implemented
                        }
                    }
                }*/

				return col;
			}
			ENDCG
		}
	}
}