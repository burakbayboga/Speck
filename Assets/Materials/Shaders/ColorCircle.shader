Shader "Custom/ColorCircle"
{
	Properties
	{
		[NoScaleOffset] _MainTex ("Texture", 2D) = "white" { }
		_FactorB ("Blue Coefficient", Range(0.5, 2)) = 1
		_FactorG ("Green Coefficient", Range(0.5, 2)) = 1
		_FactorR ("Red Coefficient", Range(0, 1)) = 0.7
	}

	SubShader
	{
		Blend One OneMinusSrcAlpha
		ZWrite Off

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
			half _FactorG;
			half _FactorB;
			half _FactorR;

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
				pixelColor.b = _FactorB * lerpParameter;

				lerpParameter = abs(i.uv.y - _Center.y) * (1 / _HalfSliceSize);
				fixed g = lerp(0, 1, lerpParameter);
				pixelColor.g = _FactorG * lerpParameter;

				pixelColor.r = _FactorR;

				pixelColor.rgb *= pixelColor.a;
				return pixelColor;
			}

			ENDCG

		}
	}
}
