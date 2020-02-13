Shader "Custom/texturewave"
{
    Properties
    {
        _BaseTex ("Base Texture", 2D) = "white" {}
        _EffectTex("Effect Texture", 2D) = "white" {}
        _Speed("Wave Speed", float) = 1
        _BaseTint("Base Tint", Color) = (1, 1, 1, 1)
        _EffectTint("Effect Tint", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            

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
                float4 localPos : TEXCOORD1;
            };

            sampler2D _BaseTex;
            float4 _BaseTex_ST;
            float _Speed;
            sampler2D _EffectTex;
            fixed4 _BaseTint;
            fixed4 _EffectTint;
            float2 _EffectCenters[4];
            float _ActiveEffectCenters[4];
            float2 _TempCenter;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _BaseTex);
                o.localPos = v.vertex;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 baseColor = tex2D(_BaseTex, i.uv) + _BaseTint;
                fixed4 effectColor = tex2D(_EffectTex, i.uv) + _EffectTint;
                
                float totalSineValue = 0;

                for (float index = 0; index < 4; index += 1)
                {
                    float dist = distance(i.uv, _EffectCenters[index]);
                    float distMapped = dist * UNITY_FOUR_PI;
                    float sine = sin(distMapped - _Time.z * _Speed);
                    sine = clamp(sine, 0, 1);
                    sine *= _ActiveEffectCenters[index];
                    totalSineValue += sine * sine * sine;
				}


                
                float effectWave = clamp(totalSineValue, 0, 1); //somewhat smooth transition
                //float effectWave = ceil(clamp(sine, 0, 1)); //sudden transition

                //effectColor = lerp(baseColor, effectColor, effectWave); 
                effectColor = lerp(baseColor, effectColor, totalSineValue * 0.6);

                return baseColor * effectColor;
            }
            ENDCG
        }
    }
}
