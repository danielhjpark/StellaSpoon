Shader "Fristy/Nature/Rocks"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,0)
        [NoScaleOffset]_MainTex ("Albedo (RBG) and Occlusion (A)", 2D) = "white" {}
        _brightness("Brightness",Range(0,2))=1
        [Space(20)]

        [NoScaleOffset][Normal]_MainNorm ("Normal ", 2D) = "bump" {}
        _MainNormPow("Normal power", Range(-2,2)) = 1
        _uv("uv", float) = 1
        [Space(50)]

        _dexTex("Detail Albedo", 2D) = "white"{}
        _dexTexPow("Blend Albedo", Range(0,2)) = 0.5
        _uv2("uv", float)=1
        [Space(20)]

        [NoScaleOffset][Normal]_DexNorm ("Detail Normal ", 2D) = "bump" {}
        _uv3("uv", float)=1
        _DexNormPow("Normal power", Range(-2,2)) = 1
        [Space(20)]

        _occlusionPow ("Occlusion Power", Range(0,1)) = 0.5
        _Glossiness ("Smoothness highs", Range(0,1)) = 0.5
        _Glossiness1 ("smoothness lows", Range(0,1)) = 0.3

        // ✅ 추가된 발광 관련 프로퍼티
        [NoScaleOffset]_RuneMaskTex ("Rune Emission Mask", 2D) = "black" {}
        _EmissionColor ("Emission Color", Color) = (1, 1, 0.5, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex, _MainNorm, _dexTex, _DexNorm;
        sampler2D _RuneMaskTex; // ✅ 추가

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_RuneMaskTex; // ✅ 추가
        };

        half _Glossiness, _Glossiness1, _MainNormPow, _uv, _occlusionPow, _brightness, _DexNormPow, _uv2, _uv3, _dexTexPow;
        fixed4 _Color;
        fixed4 _EmissionColor; // ✅ 추가

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void Unity_Blend_HardLight_float4(float4 Base, float4 Blend, out float4 Out, float Opacity)
        {
            float4 result1 = 1.0 - 2.0 * (1.0 - Base) * (1.0 - Blend);
            float4 result2 = 2.0 * Base * Blend;
            float4 zeroOrOne = step(Blend, 0.5);
            Out = result2 * zeroOrOne + (1 - zeroOrOne) * result1;
            Out = lerp(Base, Out, Opacity);
        }

        void Unity_Blend_Overlay_float4(float4 Base, float4 Blend, float Opacity, out float4 Out)
        {
            float4 result1 = 1.0 - 2.0 * (1.0 - Base) * (1.0 - Blend);
            float4 result2 = 2.0 * Base * Blend;
            float4 zeroOrOne = step(Base, 0.5);
            Out = result2 * zeroOrOne + (1 - zeroOrOne) * result1;
            Out = lerp(Base, Out, Opacity);
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            IN.uv_MainTex *= _uv;

            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 b = tex2D(_dexTex, IN.uv_MainTex * _uv2);
            fixed4 cl, cl2;

            Unity_Blend_HardLight_float4(c, c, cl, _brightness);

            fixed3 cnorm = UnpackNormal(tex2D(_MainNorm, IN.uv_MainTex));
            cnorm.xy *= _MainNormPow;

            Unity_Blend_Overlay_float4(cl, b, _dexTexPow, cl2);

            fixed3 cnorm2 = UnpackNormal(tex2D(_DexNorm, IN.uv_MainTex * _uv3));
            cnorm2.xy *= _DexNormPow;

            fixed sm = c;

            o.Albedo = lerp(_Color, cl2 * _Color, _Color.a);
            o.Normal = normalize(float3(cnorm.rg + cnorm2.rg, cnorm.b * cnorm2.b));
            o.Occlusion = lerp(1, c.a, _occlusionPow);
            o.Smoothness = lerp(_Glossiness, _Glossiness1, sm);
            o.Alpha = c.a;

            // ✅ 발광 텍스처에서 룬 마스크만 추출
            fixed runeMask = tex2D(_RuneMaskTex, IN.uv_RuneMaskTex).r;
            o.Emission = runeMask * _EmissionColor.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
