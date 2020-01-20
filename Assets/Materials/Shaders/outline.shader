Shader "Unlit/outline"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" { }
		_OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
		_BaseColor("Base Color", Color) = (1, 1, 1, 1)
	}

	SubShader
	{
		Cull Off
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			
			#pragma vertex vertexFunction
			#pragma fragment fragmentFunction
			#include "UnityCG.cginc"

			sampler2D _MainTex;

			struct v2f
			{
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
			};
			
			v2f vertexFunction(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			fixed4 _BaseColor;
			fixed4 _OutlineColor;
			float4 _MainTex_TexelSize;

			fixed4 fragmentFunction(v2f i) : COLOR
			{
				half4 c = _BaseColor;
				c.a = tex2D(_MainTex, i.uv).a;
				c.rgb *= c.a;

				half4 outlineColor = _OutlineColor;
				//outlineColor.a *= ceil(c.a);
				outlineColor.rgb *= outlineColor.a;

				fixed myAlpha = c.a;

				fixed upAlpha = tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y)).a
					+ tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y * 2)).a
					+ tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y * 3)).a
					+ tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y * 4)).a
					+ tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y * 5)).a
					+ tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y * 6)).a
					+ tex2D(_MainTex, i.uv + fixed2(0, _MainTex_TexelSize.y * 7)).a;

				fixed downAlpha = tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y)).a
					+ tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y * 2)).a
					+ tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y * 3)).a
					+ tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y * 4)).a
					+ tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y * 5)).a
					+ tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y * 6)).a
					+ tex2D(_MainTex, i.uv - fixed2(0, _MainTex_TexelSize.y * 7)).a;

				fixed rightAlpha = tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x, 0)).a
					+ tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x * 2, 0)).a
					+ tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x * 3, 0)).a
					+ tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x * 4, 0)).a
					+ tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x * 5, 0)).a
					+ tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x * 6, 0)).a
					+ tex2D(_MainTex, i.uv + fixed2(_MainTex_TexelSize.x * 7, 0)).a;

				fixed leftAlpha = tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x, 0)).a
					+ tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x * 2, 0)).a
					+ tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x * 3, 0)).a
					+ tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x * 4, 0)).a
					+ tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x * 5, 0)).a
					+ tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x * 6, 0)).a
					+ tex2D(_MainTex, i.uv - fixed2(_MainTex_TexelSize.x * 7, 0)).a;
				
				return lerp(c, outlineColor, ceil(clamp(downAlpha + upAlpha + leftAlpha + rightAlpha, 0, 1)) - ceil(myAlpha));
				//return lerp(outlineColor, c, ceil(upAlpha * downAlpha * rightAlpha * leftAlpha));
			}



			ENDCG
		}
	}
}
