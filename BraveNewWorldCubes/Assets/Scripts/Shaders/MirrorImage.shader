Shader "Custom/MirrorImageCubeRotating"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
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

            half4 frag (v2f i) : SV_Target
            {
                // Center the UV coordinates around (0.5, 0.5) for rotation
                float2 centeredUV = i.uv - 0.5;

                // Compute rotation matrix
                float angle = _Time.y;
                float2x2 rotationMatrix = float2x2(cos(angle), -sin(angle), sin(angle), cos(angle));

                // Apply rotation
                float2 rotatedUV = mul(centeredUV, rotationMatrix) + 0.5;

                float2 u = abs(2.0 * rotatedUV - 1.0);
                half4 O = half4(0, 0, 0, 1);
                
                for (float j = 0., t = _Time.y * 0.5; j < 46.; j++)
                {
                    O += 0.0012 / abs(abs(u.x - sin(t + j * 0.17) * 0.7) + u.y - sin(t + j * 0.1) * 0.6) * (cos(j + half4(0, 1, 2, 0)) + 0.4);
                }

                return O;
            }
            ENDCG
        }
    }
}
