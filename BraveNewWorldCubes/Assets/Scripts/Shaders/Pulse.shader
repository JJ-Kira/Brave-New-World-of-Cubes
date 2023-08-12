Shader "Custom/Pulse" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.vertex.xz; // Use vertex position for UV
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                const float3 sphereCoord = float3(i.uv, -1.0);
                float3 cubeCoord = normalize(sphereCoord);
                const float2 u = cubeCoord.xy * 0.5 + 0.5;

                fixed3 finalColor = fixed3(0.0, 0.0, 0.0); // Initialize with black

                for (float j = 0.0; j < 20.0; j++) {
                    float d = abs(length(u * u) - j * 0.04) + 0.005;
                    d = 0.004 / d;

                    const float3 col = (cos(j + float3(0, 1, 2)) + 1.0) * 0.5;

                    const float anim = smoothstep(0.35, 0.4, abs(abs(fmod(_Time.y, 2.0) - j * 0.1) - 1.0));

                    finalColor += col * d * anim;
                }

                return fixed4(finalColor, 1.0);
            }
            ENDCG
        }
    }
}
