// These are here to please the visual studio plugin
#include "Primitives.cginc"
#include "Utils.cginc"
#include "UnityCG.cginc"

float _Bpm;

float DistanceFunction(float3 pos)
{
    float d;
    //float r = abs(sin(2 * PI * _Time.y / 2.0));
    //float d1 = roundBox(repeat(pos, float3(6, 6, 6)), 1, r);
    //float d2 = sphere(pos, 3.0);
    //float d3 = floor(pos - float3(0, -3, 0));
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
    //repPos = abs(pos);
    //float d1 = 999999999999999999.0;
    //for(float i = 0; i < 15; i++){
    //    d1 = add(d1,
    //             box(rotate(repPos, _Time.z + PI*i/15, float3(1,0,0)) + float3(-7.5+i, -7.5+i,0), float3(2,1,1)));
    //}
    float3 rotPos = pos;
    for(int i = 0; i<2; i++){
        rotPos = rotate(abs(rotPos), _Time.y, float3(0,1,0));    
    }
    float d1 = RecursiveTetrahedron(rotPos/10) * 10;
    
    float height = 25;
    float speed = height * _Bpm * _Time.y / (60*2);
    //speed = 15;
    float elevator;
    float3 rpt = float3(pos.x, 
        fmod(pos.y+speed, height) - height*0.5,
        pos.z);
    float floor = box(rpt + float3(0,height/2,0), float3(100,height/2,100));
    float wall = add(
        box(rpt+float3(height/4,0,0), float3(2,height*2, 100)), 
        box(rpt+float3(-height/4,0,0), float3(2,height*2,100))
    ); 
    float back = add(wall,
      box(rpt+float3(0,0,40), float3(100,height, 5)) 
    );
    
    float3 bend = float3(
                    rpt.x+height/10*cos(rpt.z*10/height + _Time.z),
                    rpt.y+height/15*sin(rpt.z*10/height + _Time.z),
                    rpt.z
    );   
    float tube1 = sdCylinder(bend, float3(0,0,height/30)) + displacement(bend)*0.05;
    float3 bend2 = float3(
                    rpt.x+height/15*cos(rpt.z*10/height + _Time.z + PI),
                    rpt.y+height/10*sin(rpt.z*10/height + _Time.z + PI),
                    rpt.z
    );
    float tube2 = sdCylinder(bend2, float3(0,0,height/30)) + displacement(bend2)*0.05;

    float tube = add(tube1, tube2);

    d = add(back, floor);
    d = add(d, tube);


    return lerp(d, d1, 1-round(frac(_Time.x)));
}