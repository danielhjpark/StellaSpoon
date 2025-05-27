Shader "Custom/ObjectOutline"
{
     Properties
    {
        _OutLineColor("OutLine Color", Color) = (1,1,0,1)
        _OutLineWidth("OutLine Width", Range(0.001, 0.05)) = 0.01
    }
    SubShader
    {
        Tags { "Queue"="Overlay" "RenderType"="Opaque" }
        LOD 200

        Cull Front // 전면 Cull로 외곽선만
        ZWrite On
        // ZTest 기본 (LessEqual)

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float4 _OutLineColor;
            float _OutLineWidth;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v.vertex.xyz += v.normal * _OutLineWidth;
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutLineColor;
            }
            ENDCG
        }
    }
    FallBack Off
}
