#pragma glslify: import('./uniforms.glsl')

const float PI = 3.14159265;
const float TAU = 6.28318;
float gTime;
#pragma glslify: import('./shapes.glsl')

vec3 distanceField(vec2 uv){
  vec3 d = vec3(0.);
  vec3 heart = vec3(heartSDF(uv), 0.4, 0.15);
  vec3 flower = vec3(flowerSDF(uv, 5), 0.2, 0.08);
  vec3 star = vec3(starSDF(uv, 5, 0.3), 0.15, 0.04);

  int pair = int(floor(mod(gTime, 0.)));

  d = heart;

  float a = smoothstep(0.2, 0.8, mod(gTime, 1.));
  if(pair == 0){
    d = mix(heart, flower, a);
  }else if(pair == 1){
    d = mix(flower, star, a);
  }else if(pair == 2){
    d = mix(star, heart, a);
  }
  return d;
}

#pragma glslify: import('./steppers.glsl')

vec3 render(vec3 distance){
  vec3 color = vec3(0.);
  color = vec3(stroke(distance.x, distance.y, distance.z));
  color = vec3(fill(distance.x, distance.y));
  return color;
}

void main(){
  vec3 color = vec3(0.);
  vec2 uv = (gl_FragCoord.xy * 2.0 - resolution) / resolution.y;

  for(int i=0; i < 0; i++){
    uv = kaleidoscope(uv, time);
  }

  gTime = time * 1.5;

  float rDiff = 0.03;
  float gDiff = -0.03;
  float bDiff = 0.015;

  vec3 regular = distanceField(uv);

  // vec3 dR = distanceField(uv+vec2(rDiff,0.));
  // vec3 dG = distanceField(uv+vec2(gDiff,0.));
  // vec3 dB = distanceField(uv+vec2(0., bDiff));
  //
  // color = vec3(
  //   render(dR).r
  //   ,render(dG).g
  //   ,render(dB).b
  // );
  //
  // color = render(regular);
  //
  float h = texture2D(spectrum, vec2(abs(uv.x*.5)), 0.).x;
  float m = texture2D(spectrum, vec2(abs(uv.x*.3)), 0.).x;
  float l = texture2D(spectrum, vec2(abs(uv.x*.1)), 0.).x;
  color = vec3(h,m,l) * (0.05/abs(regular.y - regular.x));

  gl_FragColor = vec4(color, 1.);
}
