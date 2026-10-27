Shader "OVERBUILD/Skybox/SoftDay"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("360 Sky Panorama", 2D) = "white" {}
        _Tint ("Tint", Color) = (1,1,1,1)
        _Exposure ("Brightness", Range(0, 4)) = 1
        _Rotation ("Rotation", Range(0, 360)) = 0
    }
    SubShader
    {
        Tags { "Queue"="Background" "RenderType"="Background" "PreviewType"="Skybox" }
        Cull Off ZWrite Off
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            half4 _Tint;
            half _Exposure;
            float _Rotation;
            struct appdata
            {
                float4 vertex : POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            struct v2f
            {
                float4 position : SV_POSITION;
                float3 direction : TEXCOORD0;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.position = UnityObjectToClipPos(v.vertex);
                o.direction = v.vertex.xyz;
                return o;
            }
            half4 frag(v2f i) : SV_Target
            {
                float3 d = normalize(i.direction);
                float2 uv;
                uv.x = atan2(d.x, d.z) * (0.5 / UNITY_PI) + 0.5 + _Rotation / 360.0;
                uv.y = asin(clamp(d.y, -1.0, 1.0)) / UNITY_PI + 0.5;
                // Explicit LOD avoids derivative spikes where longitude wraps.
                half3 sky = tex2Dlod(_MainTex, float4(frac(uv.x), uv.y, 0, 0)).rgb;
                return half4(sky * _Tint.rgb * _Exposure, 1);
            }
            ENDCG
        }
    }
    Fallback Off
}

