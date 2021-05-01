Shader "Custom/ThreeColorSelect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color0 ("Color 0", Color) = (1, 1, 1, 1)
		_Color1 ("Color 1", Color) = (1, 1, 1, 1)
		_Color2 ("Color 2", Color) = (1, 1, 1, 1)
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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			half4 _Color0;
			half4 _Color1;
			half4 _Color2;

            v2f VertexFunction (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            half4 FragmentFunction (v2f i) : SV_Target
            {
                half4 textureColor = tex2D(_MainTex, i.uv);
				half4 pixelColor;
				pixelColor.rgb = _Color0.rgb;
				pixelColor.rgb = textureColor.r > 0.5 ? _Color1.rgb : pixelColor.rgb;
				pixelColor.rgb = textureColor.g > 0.5 ? _Color2.rgb : pixelColor.rgb;
				pixelColor.a = textureColor.a;
				pixelColor.rgb *= pixelColor.a;
				return pixelColor;
            }
            ENDCG
        }
    }
}
