Shader "Custom/BackgroundEffect"
{
    Properties
    {
        _BaseTexture ("Base Texture", 2D) = "white" { }
        _EffectTexture ("EffectTexture", 2D) = "black" { }
        _BaseTint ("Base Color", Color) = (1, 1, 1, 1)
        _EffectTint ("Effect Color", Color) = (1, 1, 1, 1)

        _WaveSpeed ("Wave Speed", Range(0, 10)) = 1
        _WaveStrengthInner ("Wave Strength Min", Range(0, 5)) = 1
        _WaveStrengthOuter ("Wave Strength Max", Range(0, 5)) = 4
        _WaveLengthInner ("Wave Length Min", Range(0, 5)) = 1
        _WaveLengthOuter ("Wave Length Max", Range(0, 5)) = 4
        _WaveOffsetInner ("Wave Offset Min", Range(-5, 5)) = 0
        _WaveOffsetOuter ("Wave Offset Max", Range(-5, 5)) = 0

    }
    SubShader
    {
        
        CGPROGRAM

        #pragma surface surf Lambert

        #include "UnityCG.cginc"

        #define MaxDistance 1.4

        sampler2D _BaseTexture;
        sampler2D _EffectTexture;
        fixed4 _BaseTint;
        fixed4 _EffectTint;

        float _WaveSpeed;
        float _WaveStrengthInner;
        float _WaveStrengthOuter;
        float _WaveLengthInner;
        float _WaveLengthOuter;
        float _WaveOffsetInner;
        float _WaveOffsetOuter;


        inline float WaveStrengthMapped(float value)
        {
            float mapNormalized = value / MaxDistance;
            return _WaveStrengthOuter * mapNormalized + _WaveStrengthInner * (1 - mapNormalized);
        }

        inline float WaveLengthMapped(float value)
        {
            float mapNormalized = value / MaxDistance;
            return _WaveLengthOuter * mapNormalized + _WaveLengthInner * (1 - mapNormalized);
		}
        
        inline float WaveOffsetMapped(float value)
        {
            float mapNormalized = value / MaxDistance;
            return _WaveOffsetOuter * mapNormalized + _WaveOffsetInner * (1 - mapNormalized);
		}

        struct Input
        {
            float2 uv_BaseTexture;
		};

        float CalculateSineValue(float distFromCenter)
        {
            return WaveStrengthMapped(distFromCenter) * sin((distFromCenter * UNITY_FOUR_PI / WaveLengthMapped(distFromCenter)) - _Time.z * _WaveSpeed) + WaveOffsetMapped(distFromCenter);
        }

        void surf(Input i, inout SurfaceOutput o)
        {
            fixed4 baseColor = tex2D(_BaseTexture, i.uv_BaseTexture) * _BaseTint;
            fixed4 effectColor = tex2D(_EffectTexture, i.uv_BaseTexture) * _EffectTint;
            
            float dist = distance(i.uv_BaseTexture, float2(0.5, 0.5));
            float distMapped = dist;
            float lerpParameter = clamp(CalculateSineValue(distMapped), 0, 1);

            o.Albedo = lerp(baseColor, effectColor, lerpParameter);
		}


        

        
        ENDCG
    }
}
