Shader "Custom/BackgroundV3"
{
    Properties
    {
		_BaseColor ("Base Color", Color) = (1, 1, 1, 1)
		_WaveColor ("Wave Color", Color) = (1, 1, 1, 1)
		_BaseWaveThickness ("Base Wave Thickness", Range(0, 5)) = 3
    }
    SubShader
    {

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
				float4 worldPos : TEXCOORD1;
            };


			half4 _BaseColor;
			half4 _WaveColor;
			half _BaseWaveThickness;

			half2 _WaveCenter[5];
			half _WaveRadiusStart[5];

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
				half lp = 0;
				[unroll]
				for (half index = 0; index < 5; index += 1)
				{
					half waveRadiusEnd = _WaveRadiusStart[index] - _BaseWaveThickness;
					half pixelRadius = distance(i.worldPos.xy, _WaveCenter[index]);
					half lpRaw = (pixelRadius - waveRadiusEnd) / (_WaveRadiusStart[index] - waveRadiusEnd);
					half lpForWave = sin(lpRaw * UNITY_PI);
					lpForWave = (lpRaw < 0 || lpRaw > 1) ? 0 : lpForWave;
					lp += lpForWave;
				}
				lp = clamp(lp, 0, 1);

				half4 pixelColor = lerp(_BaseColor, _WaveColor, lp);
				return pixelColor;
			}

            ENDCG
        }
    }
}
