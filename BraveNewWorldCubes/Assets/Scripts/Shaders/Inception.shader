Shader "Custom/CubeInCube"
{
    Properties
    {
        _MainTex("Main Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 hash4(int n)
            {
                return frac(sin(float4(532.894,392.843,402.942,837.098)*float(n+1))*4444.5);
            }

            void qrot(inout float3 v, in float4 q)
            { 
                v += 2.0 * cross(cross(v, q.xyz) + q.w * v, q.xyz);
            }

            float3 cube_ray_hit(float r, float4 q, float3 c, float3 ori, float3 rd)
            {
                ori -= c;
                qrot(ori, q);
                qrot(rd, q);
                float3 h0 = -(ori + sign(rd) * r) / rd;
                float3 h1 = -(ori - sign(rd) * r) / rd;
                float t0 = max(h0.x, max(h0.y, h0.z));
                const float t1 = min(h1.x, min(h1.y, h1.z));
                if (t1 < t0) return float3(-1, -1, -1);
                if (t0 < 1e-9) return float3(-1, -1, -1);

                float u = 1.0;
                if (h0.y > h0.x) u = 2.0;
                if (h0.z > max(h0.x, h0.y)) u = 3.0;

                float3 hp = abs(ori + rd * t0);

                float d = r - min(max(hp.x, hp.y), min(max(hp.y, hp.z), max(hp.z, hp.x)));
                return float3(t0, u, d);
            }

            float3 osc(float3 u)
            {
                u = fmod(u, 1.0) * 4.0;
                return abs(u - 2.0) - 1.0;
            }

            void getColor(out float4 fragColor, float3 pos, float3 ray)
            {
                const int depth = 10;
                int idx = 0;
                int ri = 0;
                for (int d = 0; d < depth; d++)
                {
                    float col = 0.2;
                    const float4x4 back = float4x4(0, 0, 0, 1, col, 0, 0, 1, 0, col, 0, 1, 0, 0, col, 1);
                    const float4x4 edge = float4x4(1, 1, 1, 1, 1, 0, 0, 1, 0, 1, 0, 1, 0, 0, 1, 1);

                    const float t = _Time.y * 0.2;
                    const float4 u = normalize(hash4(ri));
                    float4 v = hash4(ri + 1);
                    v = normalize(v - dot(v, u) * u);
                    const float4 q = cos(t) * u + sin(t) * v;
                    const float r = pow(0.5, float(d)) * 0.5;
                    const float3 c = (0.5 - r) * osc(t + hash4(ri + 2).xyz) * 0.3;

                    float3 hit = cube_ray_hit(r, q, c, pos, ray);
                    if (hit.y < -0.5)
                    {
                        fragColor = float4(0, 0, 0, 0); // Transparent color
                        return;
                    }
                    if (hit.z < 0.02 * r)
                    {
                        fragColor = edge[idx ^ int(hit.y)];
                        return;
                    }
                    idx ^= int(hit.y);
                    ri = ri * 3 + int(hit.y);
                }
            }

            half4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv * 2.0 - 1.0; // Map UVs to [-1, 1] range

                const float edgeThreshold = 0.005; // Adjust this value to control the thickness of the edge lines
                if (abs(uv.x) > 1.0 - edgeThreshold || abs(uv.y) > 1.0 - edgeThreshold)
                {
                    return half4(1, 1, 1, 1); // White color for edges
                }

                const float3 pos = float3(0, 0, 3);
                const float3 eye = -normalize(pos);
                const float3 up = float3(0.0, 1.0, 0.0);
                const float3 right = cross(eye, up);
                const float angle = 0.3;
                float3 ray = eye + (uv.x * right + uv.y * up) * angle;
                ray = normalize(ray);

                half4 col;
                getColor(col, pos, ray);
                return col;
            }

            ENDCG
            Blend SrcAlpha OneMinusSrcAlpha
        }
    }
}
