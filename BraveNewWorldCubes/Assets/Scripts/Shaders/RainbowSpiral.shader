Shader "Custom/SpiralRainbow"
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
                const float2 u = i.uv * 2.0 - 1.0;
                const float time = _Time.y;

                fixed3 color = fixed3(0.0, 0.0, 0.0);  // Initialize color to black
                const float2 origins[4] = {
                    float2(-1, -1),
                    float2(1, -1),
                    float2(-1, 1),
                    float2(1, 1)
                };
                            
                for (int j = 0; j < 4; j++) 
                {
                    float2 F = u - origins[j];
                    float x = 0.3;
                    const float l = length(F) + x;
                            
                    for (int k = 0; k < 12; k++)
                    {
                        fixed3 spiralColor = length(1.0/abs(F))/3e2 * (cos(time + x + float3(0, 1, 2)) * l + l).rgb / 4.0;
                        // Only add the spiral color if it's bright enough to be noticeable
                        if (max(spiralColor.r, max(spiralColor.g, spiralColor.b)) > 0.05)
                            color += spiralColor;

                        const float angle = l * 0.2 - x * time/1e2;
                        const float2x2 rotationMatrix = {
                            cos(angle), sin(angle),
                            -sin(angle), cos(angle)
                        };
                        F = mul(rotationMatrix, F);
                        x++;
                    }
                }

                return fixed4(color, 1.0);
            }

            ENDCG
        }
    }
}
