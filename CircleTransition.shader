Shader "Unlit/CircleTransition"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Radius ("Radius", Range(0, 1)) = 0
        
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha

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

            float4 _Color;

            float _Radius;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float drawCircle(float2 uv, float2 center, float radius, float smoothValue)
            {
                float sqrDistance = (uv.x - center.x) * (uv.x - center.x) + (uv.y - center.y) * (uv.y - center.y);
                float sqrRadius = radius * radius;

                if(sqrDistance < sqrRadius)
                {
                    return smoothstep(sqrRadius, sqrRadius - smoothValue, sqrDistance);
                }
                return 0;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5);
                float smoothValue = 0.01;
                float alpha = drawCircle(i.uv, center, _Radius, smoothValue);
                return float4(_Color.rgb, 1 - alpha);
            }
            ENDCG
        }
    }
}
