Shader "Custom/BlackHoleScreenEffect"
{

	Properties
	{
		_MainTex("Texture", 2D) = "white" { }
		_NoiseTex("Noise Texture (RG)", 2D) = "white" {  }
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
			float4 _BlackHoleData[2];	//x,y = world pos   z = waveRadiusStart   w = waveRadiusEnd

			v2f VertexFunction(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				return o;
			}

			half4 FragmentFunction(v2f i) : SV_Target
			{
				i.worldPos = float2(i.worldPos.x * 50 - 25, i.worldPos.y * 28 - 14);	// 何 the fuck
				float2 totalDistortion = float2(0, 0);
				float brightnessFactor = 0;
				[unroll]
				for (half index = 0; index < 2; index += 1)
				{
					float2 pixelToCenter = _BlackHoleData[index].xy - i.worldPos.xy;
					float pixelRadius = length(pixelToCenter);
					half applyDistortion = pixelRadius > _BlackHoleData[index].z && pixelRadius < _BlackHoleData[index].w;
					float2 distortionVector = applyDistortion ? normalize(pixelToCenter) / 8 : half2(0, 0);
					//float2 distortionVector = applyDistortion ? pixelToCenter / 10 : half2(0, 0);
					float t = (pixelRadius - _BlackHoleData[index].z) / (_BlackHoleData[index].w - _BlackHoleData[index].z);
					brightnessFactor += applyDistortion ? 1 - t : 0;
					t -= 0.5;
					float scale = applyDistortion ? t : 1;
					totalDistortion += distortionVector * scale;
				}
				brightnessFactor = clamp(brightnessFactor, 0, 1);
				half distortionExists = totalDistortion != float2(0, 0);

				float2 noise = tex2D(_NoiseTex, i.uv).rg;
				totalDistortion = distortionExists ? totalDistortion + (noise * 2 - 1) / 8 : 0;
				float2 finalUv = i.uv - totalDistortion;
				
				half4 color = tex2D(_MainTex, finalUv);
				float brightnessCoefficient = lerp(1, 1.2, brightnessFactor);
				color = distortionExists ? color * brightnessCoefficient : color;
				return color;
			}


			ENDCG
		}
	}

}
