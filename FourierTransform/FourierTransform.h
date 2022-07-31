#pragma once
#include <cstdint>

class FourierTransform
{
    public:
        static int16_t* GetFFT(uint16_t* inputArray);
        static int16_t* GetDFT(uint16_t* inputArray);
};

