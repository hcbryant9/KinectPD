Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _ColorA("Color A", Color) = (1, 0, 0, 1)
        _ColorB("Color B", Color) = (0, 0, 1, 1)
        _Speed("Animation Speed", Range(0, 10)) = 1
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
                float3 color : COLOR0;
            };

            float4 _ColorA;
            float4 _ColorB;
            float _Speed;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.color = lerp(_ColorA, _ColorB, (sin(_Time.y * _Speed) + 1) * 0.5); // Animate colors based on time
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                return half4(i.color.rgb, 1);
            }
            ENDCG
        }
    }

}
