#ifndef primitives_h
#define primitives_h

float sphere(float3 pos, float radius)
{
    return length(pos) - radius;
}

float roundBox(float3 pos, float3 size, float round)
{
    return length(max(abs(pos) - size * 0.5, 0.0)) - round;
}

float box(float3 pos, float3 size)
{
    return roundBox(pos, size, 0);
}

float torus(float3 pos, float2 radius)
{
    float2 r = float2(length(pos.xy) - radius.x, pos.z);
    return length(r) - radius.y;
}

float floor(float3 pos)
{
    return dot(pos, float3(0.0, 1.0, 0.0)) + 1.0;
}

float cylinder(float3 pos, float2 r)
{
    float2 d = abs(float2(length(pos.xy), pos.z)) - r;
    return min(max(d.x, d.y), 0.0) + length(max(d, 0.0)) - 0.1;
}

	
float sdCylinder( float3 p, float3 c )
{
  return length(p.xy-c.xy)-c.z;
}

float sdEllipsoid( in float3 p, in float3 r )
{
    return (length( p/r ) - 1.0) * min(min(r.x,r.y),r.z);
}

float displacement(float3 p){
    return sin(20*p.x)*sin(20*p.y)*sin(20*p.z);
}

#endif