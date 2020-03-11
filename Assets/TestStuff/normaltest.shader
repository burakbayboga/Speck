Shader "Unlit/normaltest"
{

	Properties
	{
		_MainTex ("Main Texture", 2D) = "white" { }
	}


	SubShader
	{
		CGPROGRAM

		#pragma surface surf Lambert finalcolor:mycolor

		#include "UnityCG.cginc"

		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
		};

		fixed3x3 GetRotationAroundZMatrix(float2 pixelPoint)
		{
			float2 pixelVector = pixelPoint - float2(0.5, 0.5);
			float cosTheta = dot(pixelVector, float2(1, 0)) / length(pixelVector);
			float sinTheta = 1 - cosTheta * cosTheta;

			return fixed3x3
			(
				cosTheta, -sinTheta, 0,
				sinTheta, cosTheta, 0,
				0, 0, 1
			);
		}


		fixed3 CalculateNormal(float2 pixelPoint)
		{
			
			fixed3x3 rotationAroundY = fixed3x3
			(
				0, 0, 1,
				0, 1, 0,
				-1, 0, 0
			);

			float radius = distance(pixelPoint, float2(0.5, 0.5));

			float derivative = UNITY_FOUR_PI * cos(radius * UNITY_FOUR_PI - _Time.z);

			fixed3 normal = fixed3(1, derivative, 0);

			//normal = mul(rotationAroundY, normal);

			//normal = lerp(fixed3(0, 0, 1), normal, step(0, sin(radius - _Time.z)));

			return normal;
		}

		void mycolor(Input i, SurfaceOutput o, inout fixed4 color)
		{
			fixed3x3 rotateAroundZ90 = fixed3x3
			(
				0, -1, 0,
				1, 0, 0,
				0, 0, 1
			);

			fixed3 normal = CalculateNormal(i.uv_MainTex);
			normal = mul(rotateAroundZ90, normal);
			color = fixed4(normalize(normal), 1);
		}

		void surf(Input i, inout SurfaceOutput o)
		{
			fixed3 color = tex2D(_MainTex, i.uv_MainTex);
			//o.Albedo = color;

			//o.Normal = CalculateNormal(i.uv_MainTex);

			//o.Albedo = CalculateNormal(i.uv_MainTex);
		}

		ENDCG
	}


}
