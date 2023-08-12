Shader "Custom/Geometry" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float3 palette(float t) {
                const float3 a = float3(0.5, 0.5, 0.5);
                const float3 b = float3(0.5, 0.5, 0.5);
                const float3 c = float3(1.0, 1.0, 1.0);
                const float3 d = float3(0.263, 0.416, 0.557);

                return a + b * cos(6.28318 * (c * t + d));
            }

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 frag(v2f i) : SV_Target {
                float2 uv = (i.uv * 2.0 - _MainTex_ST.xy) / _MainTex_ST.y;
                const float2 uv0 = uv;
                float3 finalColor = float3(0.0, 0.0, 0.0);

                for (float j = 0.0; j < 4.0; j++) {
                    uv = frac(uv * 1.5) - 0.5;

                    float d = length(uv) * exp(-length(uv0));

                    const float3 col = palette(length(uv0) + j * 0.4 + _Time.y * 0.4);

                    d = sin(d * 8.0 + _Time.y) / 8.0;
                    d = abs(d);

                    d = pow(0.01 / d, 1.2);

                    finalColor += col * d;
                }

                return fixed4(finalColor, 1.0);
            }
            ENDCG
        }
    }
}
