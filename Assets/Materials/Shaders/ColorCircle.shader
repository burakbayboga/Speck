Shader "Custom/ColorCircle"
{
	Properties
	{
		[NoScaleOffset] _MainTex ("Texture", 2D) = "white" { }
	}

	SubShader
	{
		Blend One OneMinusSrcAlpha

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
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _Center;
			float _HalfSliceSize;

			v2f VertexFunction(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 FragmentFunction(v2f i) : SV_TARGET
			{
				fixed4 pixelColor;
				pixelColor.a = tex2D(_MainTex, i.uv).a;

				float lerpParameter = abs(i.uv.x - _Center.x) * (1 / _HalfSliceSize);
				fixed b = lerp(0, 1, lerpParameter);
				pixelColor.b = 1.7 * lerpParameter;

				lerpParameter = abs(i.uv.y - _Center.y) * (1 / _HalfSliceSize);
				fixed g = lerp(0, 1, lerpParameter);
				pixelColor.g = 1.7 * lerpParameter;

				pixelColor.r = 0.6;

				pixelColor.rgb *= pixelColor.a;
				return pixelColor;
			}

			ENDCG

		}
	}
}
