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


        inline float StrengthMapped(float x)
        {
            float mapNormalized = x / MaxDistance;
            return _WaveStrengthOuter * mapNormalized + _WaveStrengthInner * (1 - mapNormalized);
        }

        inline float StengthMappedDerivative()
        {
            return (_WaveStrengthOuter - _WaveStrengthInner) / MaxDistance;
		}

        inline float LengthMapped(float x)
        {
            float mapNormalized = x / MaxDistance;
            return _WaveLengthOuter * mapNormalized + _WaveLengthInner * (1 - mapNormalized);
		}

        inline float LengthMappedDerivative()
        {
            return (_WaveLengthOuter - _WaveLengthInner) / MaxDistance;
		}
        
        inline float OffsetMapped(float x)
        {
            float mapNormalized = x / MaxDistance;
            return _WaveOffsetOuter * mapNormalized + _WaveOffsetInner * (1 - mapNormalized);
		}

        inline float OffsetMappedDerivative()
        {
            return (_WaveOffsetOuter - _WaveOffsetInner) / MaxDistance;
		}

        inline float TrigParameter(float x)
        {
              return x * UNITY_FOUR_PI / LengthMapped(x) - _Time.y * _WaveSpeed;
		}

        inline float TrigParameterDerivative(float x)
        {
              float lengthMapped = LengthMapped(x);
              return (lengthMapped * UNITY_FOUR_PI - LengthMappedDerivative() * UNITY_FOUR_PI * x) / (lengthMapped * lengthMapped);
		}

        struct Input
        {
            float2 uv_BaseTexture;
            float3 worldNormal;
		};

        inline fixed3x3 GetRotationAroundZMatrix(float2 pixelPoint, float2 center)
        {
            float2 pixelVector = pixelPoint - center;
            float cosTheta = dot(pixelVector, fixed2(1, 0)) / length(pixelVector);
            float sinTheta = 1 - cosTheta * cosTheta;

            return fixed3x3
                    (
                        cosTheta, -sinTheta, 0,
                        sinTheta, cosTheta, 0,
                        0, 0, 1
                    );
		}

        void CalculateSineValue(float2 pixelPoint, out float finalSine, out fixed3 finalNormal)
        {
            float sine = 0;
            float radius = 0;
            float spreadCoefficient = 0;
            float derivative = 0;
            fixed3 normal = fixed3(1, 0, 1);
            finalNormal = fixed3(0, 0, 1);
            finalSine = 0;
            fixed3 normalHelper = fixed3(1, 0, 0);
            float theta;

            fixed3x3 rotationAroundY = fixed3x3
                    (
                        0, 0, 1,
                        0, 1, 0,
                        -1, 0, 0
                    );

            for (float index = 0; index < 4; index += 1)
            {
                radius = distance(pixelPoint, _EffectCenters[index]);                
                sine = StrengthMapped(radius) * sin(TrigParameter(radius)) + OffsetMapped(radius);
                sine = saturate(sine);

                // prevents drawing inactive effects. array value is zero if inactive, 1 if active
                sine *= _ActiveEffectCenters[index];

                // start drawing from effect center
                spreadCoefficient = step(radius * _WaveSpreadCoefficient, _Time.y - _EffectStartTimes[index]);
                sine *= spreadCoefficient;
                
                finalSine += sine;

                derivative = StengthMappedDerivative() * sin(TrigParameter(radius)) + StrengthMapped(radius) * TrigParameterDerivative(radius) * cos(TrigParameter(radius)) + OffsetMappedDerivative();

                normalHelper.z = derivative;

                normal = mul(rotationAroundY, normalHelper);
                normal = mul(GetRotationAroundZMatrix(pixelPoint, _EffectCenters[index]), normal);

                finalNormal += lerp(fixed3(0, 0, 0), normalize(normal), ceil(sine));
                //finalNormal += normalize(normalHelper);


                //finalNormal += normalize(normal);

			}

            finalNormal = normalize(finalNormal)*10;

            //fixed normalPicker = ceil(saturate(finalSine));

            //finalNormal = lerp(fixed3(0, 0, 1), finalNormal, normalPicker);
        }


        void surf(Input i, inout SurfaceOutput o)
        {
            fixed3 baseColor = tex2D(_BaseTexture, i.uv_BaseTexture) * _BaseTint;
            fixed3 effectColor = tex2D(_EffectTexture, i.uv_BaseTexture) * _EffectTint;

            float sine = 0;
            fixed3 normal;

            CalculateSineValue(i.uv_BaseTexture, sine, normal);

            float lerpParameter = saturate(sine);
            //o.Albedo = lerp(baseColor, effectColor, lerpParameter);
            o.Normal = normal;



            //o.Albedo = i.worldNormal * 0.5 + 0.5;
            o.Albedo = baseColor;
            //o.Normal = fixed3(0, 0, 1);
            //o.Albedo = normal;

		}

        
        ENDCG
    }
}
