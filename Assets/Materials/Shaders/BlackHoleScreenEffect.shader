Shader "Custom/BlackHoleScreenEffect"
{

	Properties
	{
		_MainTex ("Texture", 2D) = "white" { }
		_NoiseTex ("Noise Texture (RG)", 2D) = "white" { }
		_EffectTex ("Effect Texture", 2D) = "white" { }
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
				float2 worldPos : TEXCOORD1;
			};

			sampler2D _MainTex;
			sampler2D _NoiseTex;
			sampler2D _EffectTex;
			float4 _BlackHoleData[2];	//x,y = world pos   z = waveRadiusStart   w = waveRadiusEnd

			v2f VertexFunction(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.worldPos = float2(o.worldPos.x * 50 - 25, o.worldPos.y * 28 - 14);	// classy
				return o;
			}

			half4 FragmentFunction(v2f i) : SV_Target
			{
				float2 totalDistortion = float2(0, 0);
				float brightnessFactor = 0;
				half4 effectColor = half4(0, 0, 0, 0);
				[unroll]
				for (half index = 0; index < 2; index += 1)
				{
					float2 pixelToCenter = _BlackHoleData[index].xy - i.worldPos.xy;
					float pixelRadius = length(pixelToCenter);
					float2 pixelToCenterNormalized = pixelToCenter / pixelRadius;
					half applyDistortion = pixelRadius > _BlackHoleData[index].z && pixelRadius < _BlackHoleData[index].w;
					float2 distortionVector = applyDistortion ? pixelToCenterNormalized / 8 : half2(0, 0);
					float t = (pixelRadius - _BlackHoleData[index].z) / (_BlackHoleData[index].w - _BlackHoleData[index].z);
					brightnessFactor += applyDistortion ? 1 - t : 0;
					float scale = applyDistortion ? t - 0.5 : 1;
					totalDistortion += distortionVector * scale;
					float2 effectUv = -pixelToCenterNormalized * t * 0.5 + float2(0.5, 0.5);
					effectColor += applyDistortion ? tex2D(_EffectTex, effectUv) : 0;
				}
				effectColor.rgb *= effectColor.a;

				brightnessFactor = clamp(brightnessFactor, 0, 1);
				half distortionExists = totalDistortion != float2(0, 0);

				float2 noise = tex2D(_NoiseTex, i.uv).rg;
				totalDistortion = distortionExists ? totalDistortion + (noise * 2 - 1) / 8 : 0;
				float2 finalUv = i.uv - totalDistortion;
				
				half4 color = tex2D(_MainTex, finalUv);
				float brightnessCoefficient = lerp(1, 1.2, brightnessFactor);
				brightnessCoefficient = 1;
				half4 finalBlackHoleColor = color * brightnessCoefficient + effectColor;
				color = distortionExists ? finalBlackHoleColor : color;
				return color;
			}


			ENDCG
		}
	}

}
