Shader "Unlit/EarthUnlit"
{
    Properties
    {
        _HeightMap ("Texture", 2D) = "white" {}
        _LightIntensity("LightIntensity", Float) = 2.0
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float3 worldNormal : NORMAL;
            };

            sampler2D _HeightMap;
            float _LightIntensity;
            
            float2 pointOnSphereToUV(float3 p)
            {
                p = normalize(p);

                const float PI = 3.14159;
                float latitude = asin(p.y);
                float longitude = atan2(p.z, p.x);
                    
                float u = (longitude / PI + 1) / 2;
                float v = latitude / PI + 0.5;
                return float2(u, v);
            }
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos =  mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1));
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = pointOnSphereToUV(i.worldPos);
                float4 height01 = tex2D(_HeightMap, uv);
                float3 normal = i.worldNormal;
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 light = max(0, dot(normal, lightDir)) * _LightIntensity;
                return float4(light.rgb, 1) + height01;
            }
            ENDCG
        }
    }
}
