﻿Shader "Custom/Title"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1, 1, 1, 1)
		_ShineMultiplier ("Shine Multiplier", Range(0, 1)) = 0.7
		_ShineOffset ("Shine Offset", Range(-1, 1)) = 0
		_ShineTex ("Shine Texture", 2D) = "white" {}


		[Toggle(IGNORE_TEXTURE_COLOR)] _IgnoreTextureColor ("Ignore Texture Color?", Float) = 0
    }
    SubShader
    {
		Blend One OneMinusSrcAlpha
		ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex VertexFunction
            #pragma fragment FragmentFunction
			#pragma shader_feature IGNORE_TEXTURE_COLOR

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
				half4 spriteColor : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
				float2 uvShine : TEXCOORD1;
                float4 vertex : SV_POSITION;
				half4 spriteColor : COLOR;
            };

            sampler2D _MainTex;
			sampler2D _ShineTex;
			float _ShineOffset;
			fixed _ShineMultiplier;
			fixed4 _Color;

            v2f VertexFunction (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
				o.uvShine = v.uv + float2(_ShineOffset, 0);
				o.spriteColor = v.spriteColor;
                return o;
            }

            fixed4 FragmentFunction (v2f i) : SV_Target
            {
                fixed4 textureColor = tex2D(_MainTex, i.uv);
				fixed4 pixelColor;
				pixelColor.a = textureColor.a * i.spriteColor.a;
#if defined(IGNORE_TEXTURE_COLOR)
				pixelColor.rgb = i.spriteColor.rgb * _Color.rgb * pixelColor.a;
#else
				pixelColor.rgb = i.spriteColor.rgb * _Color.rgb * textureColor.rgb * pixelColor.a;
#endif
				fixed4 shineColor = tex2D(_ShineTex, i.uvShine);
				shineColor.rgb *= pixelColor.a * shineColor.a * _ShineMultiplier;
				pixelColor.rgb += shineColor.rgb;
				
				return pixelColor;
            }
            ENDCG
        }
    }
}
