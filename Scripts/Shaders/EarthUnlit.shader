Shader "Unlit/EarthUnlit"
{
    Properties
    {
        _OceanMap ("OceanMap", 2D) = "white" {}
        _ColorMapWest ("ColorMapWest", 2D) = "white" {}
        _ColorMapEast ("ColorMapEast", 2D) = "white" {}
        _NormalMapWest ("NormalMapWest", 2D) = "white" {}
        _NormalMapEast ("NormalMapEast", 2D) = "white" {}
        _BordersMap ("BordersMap", 2D) = "white" {}
        _BordersThickness("BordersThickness", Range(0,1)) = 0.5
        [Toggle] _UseNormal("Use Normal Map", Float) = 0
        _LightIntensity("LightIntensity", Float) = 2.0
        _SpecularIntensity("SpecularIntensity", Float) = 1.0
        _ShallowWater("ShallowWater", Color) = (1, 1, 1, 1)
        _DeepWater("DeepWater", Color) = (1, 1, 1, 1)
        _NormalMap1 ("Normal Map 1", 2D) = "white" {}
        _NormalMap2 ("Normal Map 2", 2D) = "white" {}
        _Speed1 ("Speed 1", Float) = 1.0
        _Speed2 ("Speed 2", Float) = 0.5
        _Tiling1 ("Tiling 1", Float) = 1.0
        _Tiling2 ("Tiling 2", Float) = 1.0
        _WaveStrength ("WaveStrength", Float) = 1.0
        _FoamMap("FoamMap", 2D) = "white" {}
        _NoiseMap("NoiseMap", 2D) = "white" {}
        _FoamScale ("FoamScale", Float) = 1.0
        _NoiseScale("NoiseScale", Float) = 1.0
        _NoiseSpeed("NoiseSpeed", Float) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "LightMode" = "ForwardBase" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
			#include "AutoLight.cginc"

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
                float3 viewDir : TEXCOORD0;
            };

            float4 _ShallowWater;
            float4 _DeepWater;
            
            sampler2D _OceanMap;
            sampler2D _ColorMapWest;
            sampler2D _ColorMapEast;
            sampler2D _NormalMapWest;
            sampler2D _NormalMapEast;
            sampler2D _MainTex;
            sampler2D _NormalMap1;
            sampler2D _NormalMap2;
            sampler2D _FoamMap;
            sampler2D _NoiseMap;
            sampler2D _BordersMap;
            
            float _LightIntensity;
            float _UseNormal;
            float _SpecularIntensity;
            float _Speed1;
            float _Speed2;
            float _Tiling1;
            float _Tiling2;
            float _WaveStrength;
            float _FoamScale;
            float _NoiseScale;
            float _NoiseSpeed;
            float _BordersThickness;
            
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
                o.viewDir = WorldSpaceViewDir(v.vertex);
                return o;
            }

            float4 getHeightFromUV(float2 uv)
            {
                if(uv.x <= 0.5) return tex2D(_ColorMapWest, uv * float2(2, 1));
                
                return tex2D(_ColorMapEast, float2(2 * uv.x - 1, uv.y));
            }

            float4 getNormalFromUV(float2 uv)
            {
                if(uv.x <= 0.5) return tex2D(_NormalMapWest, uv * float2(2,1));
                
                return tex2D(_NormalMapEast, float2(2 * uv.x - 1, uv.y));
            }
            
            float calculateGrayScale(float3 color)
            {
                return (color.r + color.g + color.b) / 3.0;
            }

            float calculateSpecular(float3 normal, float3 viewDir, float3 dirToSun, float smoothness) {
				float specularAngle = acos(dot(normalize(dirToSun - viewDir), normal));
				float specularExponent = specularAngle / smoothness;
				float specularHighlight = exp(-max(0,specularExponent) * specularExponent);
				return specularHighlight;
			}
            
            float4 calculateOceanColor(float2 uv)
            {
                float3 color = tex2D(_OceanMap, uv);
                float height = calculateGrayScale(color);
                return lerp(_DeepWater, _ShallowWater, height);
            }
            
            float4 calculateOcean(v2f i)
            {
                float2 uv = pointOnSphereToUV(i.worldPos);
                
                float2 uvX = i.worldPos.zy;
                float2 uvY = i.worldPos.xz;
                float2 uvZ = i.worldPos.xy;
                float3 blendWeight = abs(i.worldNormal);
                blendWeight /= dot(blendWeight, 1);
                float3 viewDir = normalize(i.worldPos - _WorldSpaceCameraPos.xyz);
                
                float2 uv1X = uvX * _Tiling1 + _Time.y * _Speed1;
                float2 uv1Y = uvY * _Tiling1 + _Time.y * _Speed1;
                float2 uv1Z = uvZ * _Tiling1 + _Time.y * _Speed1;
                float2 uv2X = uvX * _Tiling2 - _Time.y * _Speed2;
                float2 uv2Y = uvY * _Tiling2 - _Time.y * _Speed2;
                float2 uv2Z = uvZ * _Tiling2 - _Time.y * _Speed2;

                float4 normal1X = tex2D(_NormalMap1, uv1X);
                float4 normal1Y = tex2D(_NormalMap1, uv1Y);
                float4 normal1Z = tex2D(_NormalMap1, uv1Z);
                float4 normal2X = tex2D(_NormalMap2, uv2X);
                float4 normal2Y = tex2D(_NormalMap2, uv2Y);
                float4 normal2Z = tex2D(_NormalMap2, uv2Z);

                float4 normal1 = normal1X * blendWeight.x + normal1Y * blendWeight.y + normal1Z * blendWeight.z;
                float4 normal2 = normal2X * blendWeight.x + normal2Y * blendWeight.y + normal2Z * blendWeight.z;
                
                float3 blendedNormal = normalize(normal1.rgb * 2.0 - 1.0 + normal2.rgb * 2.0 - 1.0);

                float3 lightDir = normalize(_WorldSpaceLightPos0);
                float diff = max(0, dot(blendedNormal, -lightDir));

                float specular = saturate(calculateSpecular(i.worldNormal, viewDir, lightDir, _SpecularIntensity));
                specular = (specular * diff >= 0.7 ? 1 : 0);
                
                float4 foam = tex2D(_FoamMap, uv);
                foam = 1 - foam;
                foam = pow(foam, _FoamScale);
                float foamGray = calculateGrayScale(foam);
                foam = float4(foamGray, foamGray, foamGray, 1);
                
                float2 noiseUV = uv * _NoiseScale + _Time * _NoiseSpeed;
                float2 noiseOffset = tex2D(_NoiseMap, noiseUV).rg;
                float2 newUV = uv + noiseOffset;
                float4 foamWithNoise = tex2D(_FoamMap, newUV);

                foamWithNoise = (foam >= _WaveStrength) ? foamWithNoise : 0;
                
                float3 col = calculateOceanColor(uv) * diff + specular * diff;
                return float4(col, 1) + foam + foamWithNoise;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = pointOnSphereToUV(i.worldPos);
                float4 height01 = getHeightFromUV(uv);
                float3 normal = (_UseNormal == 1) ? getNormalFromUV(uv) : i.worldNormal;
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 light = max(0, dot(normal, lightDir)) * _LightIntensity;

                float4 borderColor = tex2D(_BordersMap, uv);
                if(borderColor.x >= 1 - _BordersThickness) return float4(0,0,0,1);
                
                if(height01.r == 0) {
                    float3 oceanLight = max(0, dot(i.worldNormal, lightDir)) * _LightIntensity / 4;
                    
                    return calculateOcean(i) + float4(oceanLight.rgb, 1);
                }
                return float4(light.rgb, 1) + height01;
            }
            ENDCG
        }
    }
}
