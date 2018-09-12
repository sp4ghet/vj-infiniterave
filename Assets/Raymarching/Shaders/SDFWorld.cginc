float DistanceFunction(float3 pos)
{
    //float r = abs(sin(2 * PI * _Time.y / 2.0));
    //float d1 = roundBox(repeat(pos, float3(6, 6, 6)), 1, r);
    //float d2 = sphere(pos, 3.0);
    float d3 = floor(pos - float3(0, -3, 0));
    //return smoothMin(smoothMin(d1, d2, 1.0), d3, 1.0);
    //float3 warpPos = repeat(pos, 5);
    //warpPos = twistY(warpPos, 2*sin(_Time.x*10)); 
    //warpPos = twistX(warpPos, sin(_Time.x*10));
    //warpPos = rotate(warpPos, _Time.z, float3(1,1,1));
    //float torusT = torus(warpPos, float2(1, 0.25));
    
    //float3 warpPos = float3(
    //                pos.x+2*sin(pos.y/5)*sin(pos.y/5)*cos(_Time.z),
    //                pos.y,
    //                pos.z+2*sin(pos.y/3)*cos(_Time.z/2));    
    //float d1 = sdCylinder(warpPos, float3(0,0,1));

    //float3 repPos = pos;
    //repPos = repeat(pos, 15);    
    //float d1 = 999999999999999999.0;
    //for(float i = 0; i < 15; i++){
    //    d1 = add(d1,
    //             box(rotate(repPos, _Time.z + PI*i/15, float3(1,0,0)) + float3(-7.5+i, -7.5+i,0), float3(2,1,1)));
    //}

    float3 warpPos = twistY(pos, 2*sin(_Time.y*3/4)); 
    warpPos = rotate(warpPos, _Time.z, float3(1,1,1));
    float torusT = torus(warpPos, float2(2,1));

    float3 orbit = float3(sin(_Time.x*5.145)*0.15,cos(_Time.x*5.145)*0.15,0)*7;
	float sphereT = add(sphere(pos-orbit, 2), sphere(pos+orbit, 2))
            + 0.1*displacement(pos);
    
    float d1 = subtraction(torusT, sphereT);
    //float d1 = torusT;
    d1 = lerp(d1, subtraction(sphereT, torusT), (sin(_Time.y*1.14)+1)/2 - 1);

    return d1;
}