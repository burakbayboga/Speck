Shader "Custom/BlackHoleScreenEffectSimple"
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
			float4 _WorldEdgeData;	// x = xLimit * 2,  y = yLimit * 2,  z = xLimit  w = yLimit

			v2f VertexFunction(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.worldPos = float2(o.worldPos.x * _WorldEdgeData.x - _WorldEdgeData.z,
									o.worldPos.y * _WorldEdgeData.y - _WorldEdgeData.w);	// classy
				return o;
			}

			fixed4 FragmentFunction(v2f i) : SV_Target
			{
				fixed4 effectColor = 0;	// white circular lines
				fixed applyDistortion = 0;
				float distortionScale = 0;
				[unroll]
				for (fixed index = 0; index < 2; index += 1)
				{
					float2 pixelToCenter = _BlackHoleData[index].xy - i.worldPos.xy;
					float pixelRadius = length(pixelToCenter);
					float2 pixelToCenterNormalized = pixelToCenter / pixelRadius;
					fixed applyLines = pixelRadius > _BlackHoleData[index].z && pixelRadius < _BlackHoleData[index].w;
					applyDistortion += applyLines;
					float t = (pixelRadius - _BlackHoleData[index].z) / (_BlackHoleData[index].w - _BlackHoleData[index].z);
					t = applyLines ? t : 0;
					distortionScale += t > 0.5 ? 1 - t : t;
					float2 effectUv = -pixelToCenterNormalized * t * 0.5 + float2(0.5, 0.5);
					effectColor += applyLines ? tex2D(_EffectTex, effectUv) : 0;
				}
				effectColor.rgb *= effectColor.a;

				float2 noise = tex2D(_NoiseTex, i.uv).rg;
				noise = (noise * 2 - 1) / 10;
				noise *= distortionScale;
				float2 distortion = applyDistortion ? noise : 0;
				float2 finalUv = i.uv - distortion;
				
				fixed4 color = tex2D(_MainTex, finalUv);
				fixed4 finalBlackHoleColor = color + effectColor;
				return finalBlackHoleColor;
			}


			ENDCG
		}
	}

}
