Shader "Custom/Tentacle"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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

            #define PI 3.141592
            #define n (-_Time.y*.04+.03)

            float2 rot(float a, float2 v)
            {
                const float c = cos(a);
                const float s = sin(a);
                return float2(c * v.x - s * v.y, s * v.x + c * v.y);
            }

            float S(float a, float b, float d)
            {
                return lerp(a, b, 0.5 * sin(d + n * PI * 2.) + 0.5);
            }

            half4 frag (v2f i) : SV_Target
            {
                const float2 u = (i.uv - float2(0.5, 0.5)) * 2.0;
                half4 O = half4(0,0,0,1);
                for (float s = 0.; s < 3.; s++) {
                    float p = 3.;
                    for (float j = 0.; j < 25.; j++) {
                        const float angle = j * sin(n * PI * 2.) * 0.25;
                        const float2 rotatedU = rot(angle, u);
                        const float scale = j + S(1., 4., PI / 2.);
                        float2 a = frac(rotatedU * scale + 0.5) - 0.5;

                        const float r = lerp(length(a), abs(a.x) + abs(a.y), S(0., 1., 0.));
                        const float t = abs(r + 0.1 - s * 0.02 - j * S(0.005, 0.05, 0.));

                        const float step1 = smoothstep(0., 0.1 + s * j * S(0., 0.015, PI), t * S(s * 0.1 + 0.14, 0.2, 0.));
                        const float step2 = smoothstep(0., 20., j * S(0.45, 1., 0.));
                        const float step3 = smoothstep(0., 1., length(u) * j * 0.08);

                        p = min(p, step1 + step2 + step3);
                    }
                    O[int(s)] = .02/p; 
                }
                return O;
            }
            ENDCG
        }
    }
}
