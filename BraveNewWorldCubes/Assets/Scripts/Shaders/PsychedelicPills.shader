Shader "Custom/PsychedelicPills" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }

            half4 frag (v2f i) : SV_Target {
                const float elapsedTime = _Time.y;
                float p = 0.0, h = 3.0;
                const float e = elapsedTime * 0.4 + 0.8;

                float3 v = i.worldPos - _WorldSpaceCameraPos; // Direction from camera to fragment
                
                for (float s = 0.0; s < 2e2 && abs(h) > 0.001 && p < 40.0; s++) {
                    float3 o = p * normalize(float3(1.0, v.xy));
                    const float c = sin(e + p * 0.5) * 0.25;
                    const float y = c + 0.25;
                    o.x   += e; 
                    o.y    = abs(o.y);
                    o      = frac(o) - 0.5;
                    o.xy  *= float2(cos(e + float2(0, 33)));
                    o.y   += y / 2.0; 
                    o.y   -= clamp(o.y, 0.0, y);     
                    p += h = (length(o) - 0.1 * (0.75 + p * 0.1 + c)) * 0.8;
                }
                
                float3 col = exp(-p * 0.15 - 0.5 * length(v)) * (cos(p * (8.4 + 0.16 * float3(0, 1, 2))) * 1.2 + 1.2);
                return half4(col, 1.0);
            }
            ENDCG
        }
    }
}
