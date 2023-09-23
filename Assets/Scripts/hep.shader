Shader "Custom/hep"
{
    Properties
    {
        _Scale("Noise Scale", Range(0.1, 10.0)) = 1.0
        _Speed("Noise Speed", Range(0, 1)) = 0.1
    }

        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            float _Scale;
            float _Speed;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xy * _Scale + _Time.y * _Speed;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                // Generate procedural grayscale noise
                float noiseValue = frac(sin(dot(i.uv, float2(12.9898, 78.233))) * 43758.5453);
                return half4(noiseValue, noiseValue, noiseValue, 1);
            }
            ENDCG
        }
    }

}
