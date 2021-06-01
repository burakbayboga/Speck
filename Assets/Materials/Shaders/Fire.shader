Shader "Custom/Fire"
{
    Properties
    {
		_NoiseTex ("Noise Texture", 2D) = "white" { }
		_GradientTex ("Gradient Texture", 2D) = "white" { }
		_CutoffTex ("Cutoff Texture", 2D) = "white" { }
		_Color0 ("Color 0", Color) = (1, 1, 1, 1)
		_Color1 ("Color 1", Color) = (0.7, 0.7, 0.7, 1)
		_Color2 ("Color 2", Color) = (0.4, 0.4, 0.4, 1)
    }
    SubShader
    {
		Tags { "RenderType" = "Transparent" }
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM

			#pragma vertex VertexFunction
			#pragma fragment FragmentFunction
			#include "UnityCg.cginc"

			sampler2D _NoiseTex;
			sampler2D _CutoffTex;
			sampler2D _GradientTex;

			fixed4 _Color0;
			fixed4 _Color1;
			fixed4 _Color2;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			v2f VertexFunction(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 FragmentFunction(v2f i) : SV_TARGET
			{
				float noise = tex2D(_NoiseTex, i.uv - float2(0, _Time.y / 2)).r;
				float gradient = tex2D(_GradientTex, i.uv).r;

				fixed step0 = step(noise, gradient);
				fixed step1 = step(noise, gradient - 0.2);
				fixed step2 = step(noise, gradient - 0.4);

				fixed4 color = fixed4(lerp(_Color0.rgb, _Color2.rgb, step0 - step1), step0);
				color.rgb = lerp(color.rgb, _Color1, step1 - step2);
				float2 uvCutoff = i.uv;
				float xOffset = uvCutoff.y * uvCutoff.y * sin(uvCutoff.y * 4.5 + _Time.y * 3) * 0.15;
				uvCutoff.x += xOffset;
				fixed cutoffValue = tex2D(_CutoffTex, uvCutoff).r;
				color.a *= cutoffValue;
				return color;
			}

			ENDCG
		}

    }
}
