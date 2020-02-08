Shader "Unlit/planetestshader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Strength("Wave Strength", float) = 1
        _Speed("Wave Speed", float) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "DisableBatching" = "True"}
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Strength;
            float _Speed;
            
            v2f vert (appdata v)
            {
                v2f o;

                v.vertex.y = -1 * sin(distance(v.vertex.xz, float2(2,2)) * (_Time.z * _Speed + 1)) * _Strength;

                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col.rb = float2(0.4, 0.4);
                return col;
            }
            ENDCG
        }
    }
}
