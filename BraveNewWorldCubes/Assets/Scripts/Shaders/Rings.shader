Shader "Custom/CirclePatternOnCube"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                float g = 0, o = 0, f = 3.;
                float2 n = i.uv * 2.0 - 1.0;

                while (g++ < 2e2 && f > .001)
                {
                    float3 e = o * normalize(float3(n, 1.));
                    e.z += _Time.y * 0.5;
                    const float l = floor(e.z + 0.5);
                    f = 2.0 - length(e.xy) - o * 0.1;
                    e = frac(e + 0.5) - 0.5;
                    o += f = 0.5 * max(f, length(float2(length(e.xy) - lerp(0.1, 0.5, cos(2.0 * (l + _Time.y * 0.1)) * 0.5 + 0.5), e.z)) - lerp(0.05, lerp(0.1, 0.4, cos(0.5 * (1.6 + l + _Time.y * 0.1)) * 0.5 + 0.5), cos(1.0 * (1.6 + l + _Time.y * 0.1)) * 0.5 + 0.5));
                }

                return half4((cos(o * 8.0 + float3(0, 1, 2) * 0.8) * 5.0) / exp(o * 0.2 + length(n)), 1.0);
            }
            ENDCG
        }
    }
}
