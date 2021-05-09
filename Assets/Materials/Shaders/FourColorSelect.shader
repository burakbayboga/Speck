Shader "Custom/FourColorSelect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color0 ("Color 0", Color) = (1, 1, 1, 1)
		_Color1 ("Color 1", Color) = (1, 1, 1, 1)
		_Color2 ("Color 2", Color) = (1, 1, 1, 1)
		_Color3 ("Color 3", Color) = (1, 1, 1, 1)
		_CutoffTex ("Cutoff Texture", 2D) = "white" {}
		_CutoffLimit ("Cutoff Limit", Range(0, 1)) = 1
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
			fixed4 _Color0;
			fixed4 _Color1;
			fixed4 _Color2;
			fixed4 _Color3;
			float _CutoffLimit;
			sampler2D _CutoffTex;

            v2f VertexFunction (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 FragmentFunction (v2f i) : SV_Target
            {
                fixed4 textureColor = tex2D(_MainTex, i.uv);
				fixed4 pixelColor = textureColor;
				fixed3 color0 = _Color0.rgb * (1 - textureColor.r) * (1 - textureColor.g) * (1 - textureColor.b);
				fixed3 color1 = _Color1.rgb * textureColor.r;
				fixed3 color2 = _Color2.rgb * textureColor.g;
				fixed3 color3 = _Color3.rgb * textureColor.b;
				pixelColor.rgb = color0 + color1 + color2 + color3;

				float cutoffTextureR = tex2D(_CutoffTex, i.uv).r;
				pixelColor.a = _CutoffLimit > cutoffTextureR ? pixelColor.a : 0;
				
				pixelColor.rgb *= pixelColor.a;
				return pixelColor;
            }
            ENDCG
        }
    }
}
