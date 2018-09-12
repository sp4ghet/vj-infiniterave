float4 _Scale;
float _Hit;

float DistanceFunction(float3 pos)
{
    float t = _Hit;
    float a = 6 * PI * t;
    float s = pow(sin(a), 2.0);
    
    float d2 = roundBox(
        repeat(pos, 0.5),
        0.25 - 0.25 * s,
        0.25 / length(pos * 2.0));

    float3 warpPos = twistY(pos, 2*sin(_Time.y*3/4)); 
    warpPos = rotate(warpPos, _Time.z, float3(1,1,1));
    float torusT = torus(warpPos, float2(2,1));

    float3 orbit = float3(sin(_Time.x*5.145)*0.15,cos(_Time.x*5.145)*0.15,0)*7;
    float sphereT = add(sphere(pos-orbit, 2), sphere(pos+orbit, 2));
    
    float d1 = subtraction(torusT, sphereT);
    d1 = lerp(d1, subtraction(sphereT, torusT), (sin(_Time.y*1.14)+1)/2 - 1);
    
    return lerp(d1,d2, _Hit);
}