Shader "Custom/Squares"
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
                float2 u = i.uv * 2.0 - 1.0;

                fixed3 color = fixed3(0.0, 0.0, 0.0);
                const float time = _Time.y;

                for (float j = 0.0; j < 20.0; j++)
                {
                    color += (0.004 / (abs(length(u * u) - j * 0.04) + 0.005)) 
                            * (cos(j + fixed3(0, 1, 2)) + 1.0) 
                            * smoothstep(0.35, 0.4, abs(abs(fmod(time, 2.0) - j * 0.1) - 1.0));
                    const float speed = 0.1;
                    const float2x2 rotationMatrix = float2x2(cos(time * speed + j * 0.03), -sin(time * speed + j * 0.03), sin(time * speed + j * 0.03), cos(time * speed + j * 0.03));
                    u = mul(u, rotationMatrix);
                }

                return fixed4(color, 1.0);
            }

            ENDCG
        }
    }
}
