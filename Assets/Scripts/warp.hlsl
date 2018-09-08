#include "PostProcessing/Shaders/StdLib.hlsl"
#include "Assets/Scripts/3dnoise.hlsl"
TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);

float my_noise(float2 p){
    return cnoise(float3(p, _Time.z)*5.);
}

float2 nabla(float2 p){
  float2 deltaX = float2(0.001, 0.);
  float2 deltaY = float2(0., 0.001);

  float x = my_noise(p+deltaX) - my_noise(p-deltaX);
  float y = my_noise(p+deltaY) - my_noise(p-deltaY);

  float2 curled = -float2(x, y);

  //enable this to debug since the gradients are so small it's hard to visualize
  //curled = normalize(curled);

  return curled * _Blend * 50;
}

float4 Frag(VaryingsDefault attr) : SV_Target
{
    float2 displacement = nabla(attr.texcoord);
    float2 uv = fmod(attr.texcoord+displacement, float2(1,1));
    float4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
    return color;
}
