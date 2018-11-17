#ifndef RAYMARCHING
#define RAYMARCHING

#pragma glslify: import('./uniforms.glsl')

const float PI = 3.14159265;

float lengthN(vec3 v, float p)
{
  vec3 tmp = pow(abs(v), vec3(p));
  return pow(tmp.x+tmp.y+tmp.z, 1.0/p);
}
3
vec3 replicate(vec3 p){
	return mod(p, 8.0) - 4.;
}

float distanceFunction(vec3 p)
{
	vec3 b = vec3(1.);
	return lengthN(replicate(p), 4.) - 1.;
}

vec3 getNormal(vec3 p){
	const float d = 0.0001;
	return normalize(vec3(
		distanceFunction(p+vec3(d,0.,0.)) - distanceFunction(p+vec3(-d,0.,0.)),
		distanceFunction(p+vec3(0.,d,0.)) - distanceFunction(p+vec3(0.,-d,0.)),
		distanceFunction(p+vec3(0.,0.,d)) - distanceFunction(p+vec3(0.,0.,-d))
	));
}

void main() {
	//画面の座標系を左下(-x/y, -1)→右上(x/y, 1)、中心(0,0)に変換する
	vec2 pos = (gl_FragCoord.xy * 2.0 - resolution) / resolution.y;

	//カメラ位置や向きを指定する
	vec3 camPos = vec3(0.0, 0.0, 3.0*time);
 	vec3 camDir = vec3(0.0, 0., -1.0);
	vec3 camUp = vec3(0.0, 1.0, 0.0);
	vec3 camSide = cross(camDir, camUp);
	float focus = 1.8;

	//画面のピクセルに応じてRayを放つ
	vec3 rayDir = normalize(camSide*pos.x + camUp*pos.y + camDir*focus);

	float t = 0.0,
		d;
	vec3 posOnRay = camPos;

	for(int i=0; i<64; i++)
	{
	d = distanceFunction(posOnRay);
	t += d;
	posOnRay = camPos + t*rayDir;
	}


	// vec3 normal = getNormal(posOnRay);
	if(abs(d) < 0.1)
	{
	gl_FragColor = vec4(abs(pos.x), abs(pos.y), 1., 1.);
	}else
	{
	gl_FragColor = vec4(0.0);
	}
}

#endif
