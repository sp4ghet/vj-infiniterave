Shader "Raymarching/World"
{

Properties
{
    _MainTex ("Main Texture", 2D) = "" {}
}

SubShader
{

Tags { "RenderType" = "Opaque" "DisableBatching" = "True" "Queue" = "Geometry+10" }
Cull Off

Pass
{
    Tags { "LightMode" = "Deferred" }

    Stencil
    {
        Comp Always
        Pass Replace
        Ref 255
    }

    CGPROGRAM
    #pragma vertex vert
    #pragma fragment frag
    #pragma target 3.0
    #pragma multi_compile ___ UNITY_HDR_ON

    #include "UnityCG.cginc"
    #include "Utils.cginc"
    #include "Primitives.cginc"

    #define PI 3.14159265358979

    float4 _Scale;

    #include "SDFWorld.cginc"

    #include "Raymarching.cginc"

    sampler2D _MainTex;

    GBufferOut frag(VertOutput i)
    {
        float3 rayDir = GetRayDirection(i.screenPos);

        float3 camPos = GetCameraPosition();
        float maxDist = GetCameraMaxDistance();

        float distance = 0.0;
        float len = 0.0;
        float3 pos = camPos + _ProjectionParams.y * rayDir;
        for (int i = 0; i < 100; ++i) {
            distance = DistanceFunction(pos);
            len += distance;
            pos += rayDir * distance;
            if (distance < 0.001 || len > maxDist) break;
        }

        if (distance > 0.001) discard;

        float depth = GetDepth(pos);
        float3 normal = GetNormalOfDistanceFunction(pos);

        float u = (1.0 - floor(fmod(pos.x, 2.0))) * 5;
        float v = (1.0 - floor(fmod(pos.y, 2.0))) * 5;

        GBufferOut o;
        o.diffuse  = float4(1.0, 1.0, 1.0, 1.0);
        o.specular = float4(0.5, 0.5, 0.5, 1.0);
        //o.emission = tex2D(_MainTex, float2(u, v)) * 3;
        o.emission = float4(0,0,0,0);
        o.depth    = depth;
        o.normal   = float4(normal, 1.0);

#ifndef UNITY_HDR_ON
        o.emission = exp2(-o.emission);
#endif

        return o;
    }

    ENDCG
}

}

Fallback Off
}