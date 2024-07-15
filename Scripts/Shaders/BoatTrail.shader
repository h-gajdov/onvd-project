Shader "Unlit/BoatTrail"
{
    Properties
    {
        _NoiseTex ("NoiseTexture", 2D) = "white" {}
        _NoiseScale ("NoiseScale", Float) = 1.0
        _ScrollSpeed ("ScrollSpeed", Vector) = (0.1, 0.0, 0.0, 0.0)
    }
    SubShader
    {
        Tags { "Queue" = "Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard alpha:fade

        float4 _ScrollSpeed;

        sampler2D _NoiseTex;

        float _NoiseScale;

        struct Input
        {
            float2 uv_NoiseTex;
            float3 worldPos;
        };

        const float e = 2.71828;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float2 scrollUV = IN.uv_NoiseTex + _ScrollSpeed.xy * _Time;

            float distanceTraveled = IN.uv_NoiseTex.x * 10;
            float fade = saturate(1.0 - distanceTraveled / 10);
            
            fixed4 noise = tex2D(_NoiseTex, scrollUV);
            o.Albedo = (noise.r > _NoiseScale) ? float3(1,1,1) : float3(0,0,0);
            o.Alpha = (noise.r > _NoiseScale) ? fade : 0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
