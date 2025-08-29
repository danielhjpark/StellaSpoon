Shader "UI/ScreenBlur"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Size ("Blur Size", Range(0, 20)) = 5
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _Size;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                fixed4 col = tex2D(_MainTex, uv) * 0.36; // 중심

                // 좌우
                col += tex2D(_MainTex, uv + float2(_MainTex_TexelSize.x * _Size, 0)) * 0.12;
                col += tex2D(_MainTex, uv - float2(_MainTex_TexelSize.x * _Size, 0)) * 0.12;

                // 위아래
                col += tex2D(_MainTex, uv + float2(0, _MainTex_TexelSize.y * _Size)) * 0.12;
                col += tex2D(_MainTex, uv - float2(0, _MainTex_TexelSize.y * _Size)) * 0.12;

                // 대각선 4방향
                col += tex2D(_MainTex, uv + float2(_MainTex_TexelSize.x * _Size, _MainTex_TexelSize.y * _Size)) * 0.04;
                col += tex2D(_MainTex, uv - float2(_MainTex_TexelSize.x * _Size, _MainTex_TexelSize.y * _Size)) * 0.04;
                col += tex2D(_MainTex, uv + float2(-_MainTex_TexelSize.x * _Size, _MainTex_TexelSize.y * _Size)) * 0.04;
                col += tex2D(_MainTex, uv + float2(_MainTex_TexelSize.x * _Size, -_MainTex_TexelSize.y * _Size)) * 0.04;

                return col;
            }
            ENDCG
        }
    }
}


