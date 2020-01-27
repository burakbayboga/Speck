Shader "Custom/Straightline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" { }
		_CenterColor ("CenterColor", Color) = (1, 1, 1, 1)
		_OuterColor ("OuterColor", Color) = (0, 0, 0, 0)
		_Center ("Center", Vector) = (0, 0, 0, 0)
		_Threshold ("Threshold", Float) = 0.001
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
			fixed4 _CenterColor;
			fixed4 _OuterColor;
			float _Threshold;
			float4 _Center;

			v2f VertexFunction(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 FragmentFunction(v2f i) : SV_TARGET
			{
				fixed4 textureColor = tex2D(_MainTex, i.uv);
				fixed4 pixelColor;
				pixelColor.a = textureColor.a;

				float2 distVector = i.uv - _Center.xy;
				float dist = dot(distVector, distVector);
				float dif = dist - _Threshold;
				float lerpParameter = ceil(clamp(dif, 0, 1));
				fixed3 pixelRGB = lerp(_CenterColor, _OuterColor, lerpParameter);

				pixelColor.rgb = pixelRGB;
				pixelColor.rgb *= pixelColor.a;

				return pixelColor;
			}

			ENDCG

		}
	}
}
