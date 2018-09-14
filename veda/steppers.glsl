#ifndef STEPPERS
#define STEPPERS
float fill(float x, float size){
  return 1.-step(size, x);
}

float stroke(float x, float s, float w){
  float d = step(s, x+w*.5) -
            step(s, x-w*.5);
  return clamp(d, 0., 1.);
}

float flip(float v, float pct){
  return mix(v, 1.-v, pct);
}

vec2 rotate(vec2 uv, float angle){
  return mat2(cos(angle), -sin(angle),
       sin(angle), cos(angle)) * uv;
}

vec3 bridge(vec3 c, float d, float s, float w){
  c *= 1. - stroke(d,s,w*2.);
  return c + stroke(d,s,w);
}

vec2 scale(vec2 uv, vec2 s){
  return uv * s;
}

vec2 kaleidoscope(vec2 uv, float lTime){
    vec2 p = abs(uv*1.5) - 1.0;

    p = rotate(p, lTime);

    return p;
}

#endif
