//UNITY_SHADER_NO_UPGRADE

#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

// #pragma target 3.0

void CheckTextureAssigned_float(UnityTexture2D text, UnitySamplerState samp, out float4 isAssigned) {
    isAssigned = 0.0;

    float4 sample = text.Sample(samp, float2(0.5, 0.5));

    if (text.name == "None") {
        isAssigned = 0.0;
    } else if (sample.a > 0 || sample.r > 0 || sample.g > 0 || sample.b > 0) {
        isAssigned = 1.0;
    }

    // return isAssigned;
    // return isAssigned;
}

// void CheckTextureAssigned(UnitySamplerState sampler, out float isAssigned) {
    // Initialize the output
    // isAssigned = 0.0;

    // Check if the texture is null by sampling it
    // float4 sample = texture.Sample(sampler, float2(0.5, 0.5));

    // If the sample contains any color data, consider the texture assigned
    // if (sample.a > 0 || sample.r > 0 || sample.g > 0 || sample.b > 0) {
    //     isAssigned = 1.0;
    // }
// }

#endif