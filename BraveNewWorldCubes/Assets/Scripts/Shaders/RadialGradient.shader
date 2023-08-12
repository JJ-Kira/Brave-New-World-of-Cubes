Shader "Custom/RadialGradient" {
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

            v2f vert(appdata_t v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 frag(v2f i) : SV_Target {
                float3 p;
                half4 O = half4(0, 0, 0, 1); // Initialize with alpha = 1

                float2 uv = (i.uv * 2.0 - 1.0); // Adjusted UV mapping

                p = float3(uv, 1.0);
                float d = length(p);
                O.rgb = (1.0 + cos(d + _Time.y)) / 2.0;

                return O;
            }
            ENDCG
        }
    }
}
