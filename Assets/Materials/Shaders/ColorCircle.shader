Shader "Custom/ColorCircle"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" { }
		_DownColor ("Down Color", Color) = (1, 1, 1, 1)
		_UpColor ("Up Color", Color) = (0, 0, 0, 0)
		[HideInInspector] _Center ("Center", Vector) = (0, 0, 0, 0)
		[HideInInspector] _HalfSliceSize("Slice Size", Float) = 0
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
			fixed4 _DownColor;
			fixed4 _UpColor;
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

				// circular
				//float2 distVector = i.uv - _Center.xy;
				//float dist = dot(distVector, distVector);
				//float dif = dist - _Threshold;
				//float lerpParameter = ceil(clamp(dif, 0, 1));
				//fixed3 pixelRGB = lerp(_CenterColor, _OuterColor, lerpParameter);
				//pixelColor.rgb = pixelRGB;


				// up down
				//float lerpParameter = (i.uv.y - (_Center.y - _HalfSliceSize)) /
				//						(i.uv.y - (_Center.y + _HalfSliceSize));
				//
				//pixelColor = lerp(_DownColor, _UpColor, lerpParameter);
				//
				//pixelColor.a = textureColor.a;
				//pixelColor.rgb *= pixelColor.a;
				//
				//return pixelColor;

				float lerpParameter = abs(i.uv.x - _Center.x) * (1 / _HalfSliceSize);
				fixed b = lerp(0, 1, lerpParameter);
				pixelColor.b = b;

				lerpParameter = abs(i.uv.y - _Center.y) * (1 / _HalfSliceSize);
				fixed g = lerp(0, 1, lerpParameter);
				pixelColor.g = g;

				pixelColor.r = 1;

				pixelColor.rgb *= pixelColor.a;
				return pixelColor;
			}

			ENDCG

		}
	}
}
