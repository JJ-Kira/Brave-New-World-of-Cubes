Shader "Custom/Hypnotism"
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

            #define c(a) (sin(a)*.5+.5)
            #define g    (_Time.y*.5)

            float sphereSDF(float3 p, float r) {
                return length(p) - r;
            }

            half4 frag (v2f i) : SV_Target
{
    float r = 0.0, t = 3.0;
    float3 p = float3(i.uv, 1.0) * 2.0 - 1.0;
    p.z += g * 0.5; // Slow down time-based animation

    for (float a = 0.; a < 150. && t > .002*r && r < 50.; a++) {
        const float angle = lerp(c(g), -c(g), c((g-3.14)/2.)) * r;
        const float2x2 rotationMatrix = float2x2(cos(angle), -sin(angle), sin(angle), cos(angle));
        p.xy = mul(rotationMatrix, p.xy);
        r += t = max(sphereSDF(p, 0.3), -sphereSDF(p, 1.1)) * 0.2; // Reduced ray advancement
    }

    return half4(cos(float3(lerp(2.05,1.85,c(g)),2.1,2.15)*r-g) * exp2(-r*.06), 1.0);
}

            ENDCG
        }
    }
}
