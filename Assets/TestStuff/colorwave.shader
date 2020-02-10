Shader "Unlit/colorwave"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Speed("Wave Speed", float) = 1
        _StartTime("Start Time", float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


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
                float4 localPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Speed;
            float _StartTime;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.localPos = v.vertex;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 baseColor = tex2D(_MainTex, i.uv);
                fixed4 effectColor = fixed4(0.2, 0, 0.7, 1);

                float dist = distance(i.uv, fixed2(0.5, 0.5));
                
                float distMapped = dist * UNITY_FOUR_PI / 0.5;
                //try clamp(time - startTime) for singular wave pulse?


                //float sine = sin(distMapped - _Time.z * _Speed);
                float sine = sin(distMapped - clamp(_Time.x - _StartTime, 0, 8) * _Speed);

                
                float effectWave = clamp(sine, 0, 1);

                effectColor = lerp(baseColor, effectColor, effectWave);
                
                return baseColor * effectColor;
            }
            ENDCG
        }
    }
}
