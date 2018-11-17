Shader "Custom/Warp"
{
    HLSLINCLUDE
    float _Blend;
    #include "Assets/Scripts/warp.hlsl"
    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

                #pragma vertex VertDefault
                #pragma fragment Frag

            ENDHLSL
        }
    }
}