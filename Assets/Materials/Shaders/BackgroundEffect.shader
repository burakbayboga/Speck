Shader "Custom/BackgroundEffect"
{
    Properties
    {
        _BaseTexture ("Base Texture", 2D) = "white" { }
        _EffectTexture ("EffectTexture", 2D) = "black" { }
        _BaseTint ("Base Color", Color) = (1, 1, 1, 1)
        _EffectTint ("Effect Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        
        CGPROGRAM

        #pragma surface surf Lambert

        #include "UnityCG.cginc"

        sampler2D _BaseTexture;
        sampler2D _EffectTexture;
        fixed4 _BaseTint;
        fixed4 _EffectTint;

        struct Input
        {
            float2 uv_BaseTexture;
		};

        void surf(Input i, inout SurfaceOutput o)
        {
            fixed4 baseColor = tex2D(_BaseTexture, i.uv_BaseTexture) * _BaseTint;
            fixed4 effectColor = tex2D(_EffectTexture, i.uv_BaseTexture) * _EffectTint;

            o.Albedo = lerp(baseColor, effectColor, (sin(_Time.w) + 1) / 2);
		}

        
        ENDCG
    }
}
