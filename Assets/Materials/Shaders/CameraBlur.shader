﻿Shader "Custom/CameraBlur"
{

	Properties
	{
		_MainTex("Texture", 2D) = "white" { }
	}

	SubShader
	{
		Cull Off ZWrite Off ZTest Always

		Pass
		{

			CGPROGRAM

			#pragma vertex VertexFunction
			#pragma fragment FragmentFunction

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_TexelSize;

			v2f VertexFunction(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float4 blur(sampler2D tex, float2 uv, float4 size)
			{
				float4 c = tex2D(tex, uv + float2(-size.x, size.y)) + tex2D(tex, uv + float2(0, size.y)) + tex2D(tex, uv + float2(size.x, size.y)) +
					tex2D(tex, uv + float2(-size.x, 0)) + tex2D(tex, uv + float2(0, 0)) + tex2D(tex, uv + float2(size.x, 0)) +
					tex2D(tex, uv + float2(-size.x, -size.y)) + tex2D(tex, uv + float2(0, -size.y)) + tex2D(tex, uv + float2(size.x, -size.y));
				
				return c / 9;
			}

			float4 FragmentFunction(v2f i) : SV_Target
			{
				float4 color = blur(_MainTex, i.uv, _MainTex_TexelSize);
				return color;
			}


			ENDCG
		}
	}

}
