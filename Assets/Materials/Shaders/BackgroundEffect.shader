Shader "Custom/BackgroundEffect"
{
    Properties
    {
        _BaseTexture ("Base Texture", 2D) = "white" { }
        _EffectTexture ("EffectTexture", 2D) = "black" { }
        _BaseTint ("Base Color", Color) = (1, 1, 1, 1)
        _EffectTint ("Effect Color", Color) = (1, 1, 1, 1)

        _WaveSpeed ("Wave Speed", Range(0, 10)) = 1
        _WaveSpreadCoefficient ("Wave Spread Coefficient", Range(0, 8)) = 2
        _WaveStrengthInner ("Inner Wave Strength", Range(0, 5)) = 1
        _WaveStrengthOuter ("Outer Wave Strength", Range(0, 5)) = 4
        _WaveLengthInner ("Inner Wave Length", Range(0, 5)) = 1
        _WaveLengthOuter ("Outer Wave Length", Range(0, 5)) = 4
        _WaveOffsetInner ("Inner Wave Offset", Range(-5, 5)) = 0
        _WaveOffsetOuter ("Outer Wave Offset", Range(-5, 5)) = 0

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

        float2 _EffectCenters[4];
        float _ActiveEffectCenters[4];
        float _EffectStartTimes[4];

        float _WaveSpeed;
        float _WaveSpreadCoefficient;
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

        float CalculateSineValue(float2 pixelPoint)
        {
            float totalSineValue = 0;
            float sine = 0;
            float radius = 0;
            float spreadCoefficient = 0;

            for (float index = 0; index < 4; index += 1)
            {
                radius = distance(pixelPoint, _EffectCenters[index]);                
                sine = WaveStrengthMapped(radius) * sin((radius * UNITY_FOUR_PI / WaveLengthMapped(radius)) - _Time.y * _WaveSpeed) + WaveOffsetMapped(radius);
                sine = clamp(sine, 0, 1);

                // prevents drawing inactive effects. array value is zero if inactive, 1 if active
                sine *= _ActiveEffectCenters[index];

                // start drawing from effect center
                spreadCoefficient = ceil(clamp(_Time.y - _EffectStartTimes[index] - radius * _WaveSpreadCoefficient, 0, 1));
                sine *= spreadCoefficient;
                
                totalSineValue += sine;
			}

            return totalSineValue;
        }

        void surf(Input i, inout SurfaceOutput o)
        {
            fixed4 baseColor = tex2D(_BaseTexture, i.uv_BaseTexture) * _BaseTint;
            fixed4 effectColor = tex2D(_EffectTexture, i.uv_BaseTexture) * _EffectTint;
            

            float lerpParameter = clamp(CalculateSineValue(i.uv_BaseTexture), 0, 1);

            o.Albedo = lerp(baseColor, effectColor, lerpParameter);
		}


        

        
        ENDCG
    }
}
