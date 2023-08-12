Shader "Custom/DancingStarCube"
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

            fixed4 frag (v2f i) : SV_Target
            {
                float scale = 0.5; // Adjust this value to change the pattern size. Smaller values will make the pattern bigger.
                float2 u = i.uv * scale - 0.5 * (scale - 1); // Scale and center the pattern

                float2 r = float2(1.0, 1.0);
                u += u - r;

                float iter = 0.0;
                fixed4 O = fixed4(0, 0, 0, 1);

                for (O *= iter; iter < 8.0; iter += 0.05)
                {
                    u = mul(u, float2x2(cos(2.5), sin(2.5), -sin(2.5), cos(2.5)));
                    O += pow(
                        0.006/length(u/r.y/0.3+sin(iter+float2(_Time.y*0.5,_Time.y+1.0)))
                        *(cos(iter*6.0+float4(0,1,2,0))+1.0), O-O+1.95);
                }

                return O;
            }

            ENDCG
        }
    }
}
