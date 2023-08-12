Shader "Custom/NeonCore" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
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

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float3 GetColor(float t) {
                const float3 a = float3(0.8, 0.6, 0.7);
                const float3 b = float3(0.4, 0.6, 0.5);
                const float3 c = float3(0.6, 0.7, 0.5);
                const float3 d = float3(0.7 + 0.3 * sin(_Time.y), 0.5 + 0.5 * cos(_Time.y), 0.6 + 0.4 * sin(_Time.y + 1.5));
                return a + b * cos(6.28318 * (c * t + d));
            }

            fixed4 frag(v2f i) : SV_Target {
                half4 O = half4(0, 0, 0, 0); // Initialize with transparent color
                const float k = fmod(_Time.y * 0.2, 2.0 * 3.14159); // Looping time for dynamic movement

                float2 uv = i.uv * 2.0 - 1.0;

                const float3 p = float3(uv, 0.5 * sin(k * 6.0)); // Increased frequency for more dynamic movement
                const float d = length(p);
                const float ringValue = (1.0 + cos(d * 12.0 + k * 3.0)) * 0.5; // Increased frequency for more rings

                O.rgb = GetColor(d) * ringValue;
                O.a = ringValue; // Set alpha based on ring value

                // Edge detection and coloring
                const float edgeWidth = 0.02; // Thinner edge
                if (abs(uv.x) > 1.0 - edgeWidth || abs(uv.y) > 1.0 - edgeWidth) {
                    O.rgb = float3(1.0, 1.0, 1.0); // White color
                    O.a = 0.5; // Slightly transparent
                }

                return O;
            }
            ENDCG
            Blend SrcAlpha OneMinusSrcAlpha // Enable transparency blending
        }
    }
}
