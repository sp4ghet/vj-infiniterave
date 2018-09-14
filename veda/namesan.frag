#pragma glslify: import('./uniforms.glsl')

uniform sampler2D image1;

const float PI = 3.14159265;
const float TAU = 6.28318;

#pragma glslify: import('./noise.glsl')

void main(){
  float nScale = 1.;
  vec3 nStart = vec3(0,0, time);
  vec3 color = vec3(0.);
  vec3 uvz = (gl_FragCoord.xyz * 2.0 - vec3(resolution, 100.)) / resolution.y;
  vec2 uv = uvz.xy;

  vec2 xy = gl_FragCoord.xy / resolution;

  vec3 nSpace = nStart + uvz * nScale;

  float n = snoise(nSpace);
  n = (1. + n)*.5;

  // xy = xy + vec2(n*0.1, n*0.05);

  // chromatic aberattion
  float rDiff = 0.003;
  float gDiff = -0.003;
  float bDiff = 0.0015;

  color = vec3(r,g,b);

  gl_FragColor = vec4(color, 1.);
}
