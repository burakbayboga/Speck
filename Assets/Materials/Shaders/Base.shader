Shader "Custom/Base"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
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
                float4 vertex : SV_POSITION;
				half4 spriteColor : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f VertexFunction (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.spriteColor = v.spriteColor;
                return o;
            }

            fixed4 FragmentFunction (v2f i) : SV_Target
            {
                fixed4 textureColor = tex2D(_MainTex, i.uv);
				fixed4 pixelColor;
				pixelColor.a = textureColor.a;
				pixelColor.rgb = i.spriteColor.rgb * textureColor.rgb * pixelColor.a;
				return pixelColor;
            }
            ENDCG
        }
    }
}
