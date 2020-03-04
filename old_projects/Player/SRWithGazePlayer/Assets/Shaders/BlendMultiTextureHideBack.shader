// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "IZMHR/BlendMultiTextureHideBack"
{

	Properties{
		_BlendTexRatio1("Blend Tex Ratio 1", Range(0, 1)) = 0
		_BlendTexRatio2("Blend Tex Ratio 2", Range(0, 1)) = 0
		_BlendTexRatio3("Blend Tex Ratio 3", Range(0, 1)) = 0
		_BlendTexRatio4("Blend Tex Ratio 4", Range(0, 1)) = 0
		_Tint("Tint", Range(0, 1)) = 1
		_MainTex("Texture R", 2D) = ""
		_Texture2("Texture G", 2D) = ""
		_Texture3("Texture B", 2D) = ""
		_Texture4("Texture X", 2D) = ""
		_HideBack("HideBack", 2D) = ""
	}

		SubShader{
			Pass{
		//Tags {"RenderType" = "Opaque"}
		CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag
		#pragma target 2.0

		#include "UnityCG.cginc"

		struct appdata_t {
			float4 vertex : POSITION;
			float2 texcoord : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f {
			float4 vertex : SV_POSITION;
			float2 texcoord : TEXCOORD0;
			UNITY_FOG_COORDS(1)
				UNITY_VERTEX_OUTPUT_STEREO
		};

		float _BlendTexRatio1;
		float _BlendTexRatio2;
		float _BlendTexRatio3;
		float _BlendTexRatio4;
		float _Tint;
		sampler2D _MainTex;
		sampler2D _Texture2;
		sampler2D _Texture3;
		sampler2D _Texture4;
		sampler2D _HideBack;
		float4 _MainTex_ST;

		v2f vert(appdata_t v)
		{
			v2f o;
			UNITY_SETUP_INSTANCE_ID(v);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
			return o;
		}

		fixed4 frag(v2f i) : SV_Target{
			fixed4 col = tex2D(_MainTex, i.texcoord) * _BlendTexRatio1 + tex2D(_Texture2, i.texcoord) * _BlendTexRatio2 + tex2D(_Texture3, i.texcoord) * _BlendTexRatio3 + tex2D(_Texture4, i.texcoord) * _BlendTexRatio4;

		// Hide Back
		fixed4 hideBack = tex2D(_HideBack, i.texcoord);
		col = col * (hideBack.r + hideBack.g + hideBack.b) / 3.0;

		// Tint
		col = col * _Tint;
		return col;
	}
	ENDCG
}
	}
		Fallback "Diffuse"
}
