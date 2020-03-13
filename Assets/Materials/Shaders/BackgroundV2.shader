Shader "Custom/BackgroundV2"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColor ("Base Color", Color) = (1, 1, 1, 1)
        _EffectColor ("Effect Color", Color) = (1, 1, 1, 1)
        _WaveThickness ("Wave Thickness", float) = 0.1
        _WaveSpeed ("Wave Speed", float) = 2
    }

    SubShader
    {
        
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

            sampler2D _MainTex;
            fixed4 _BaseColor;
            fixed4 _EffectColor;
            float _WaveThickness;
            float _WaveSpeed;

            float2 _EffectCenters[4];
            float _ActiveEffectCenters[4];
            float _EffectStartTimes[4];
            
            v2f VertexFunction(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                return o;
			}

            fixed4 FragmentFunction(v2f i) : SV_Target
            {
                fixed4 textureColor = tex2D(_MainTex, i.uv);
                fixed4 pixelColor;
                float effectMultiplier = 1;

                [unroll]
                for (int index = 0; index < 4; index += 1)
                {
                    float radius = distance(i.worldPos.xy, _EffectCenters[index]);
                    
                    float effectWeight = (radius - _WaveSpeed * (_Time.y - _EffectStartTimes[index]) + _WaveThickness) / _WaveThickness;
                    fixed effectDecoder = step(0, effectWeight) * step(effectWeight, 1) * _ActiveEffectCenters[index];
                
                    effectMultiplier *= lerp(1, lerp(0.6, 1.2, effectWeight), effectDecoder);
				}
                
                pixelColor.rgb = _BaseColor.rgb * effectMultiplier;

                pixelColor.a = 1;

                return pixelColor;
			}


            ENDCG
		}

    }
}
