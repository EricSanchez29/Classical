#pragma once

#define _USE_MATH_DEFINES

#include <cstdint>;
#include <cmath>;

class FourierTransform
{
    public:
        static int16_t* FFT(uint16_t* inputArray);
        static int16_t* DFT(uint16_t* inputArray);
        static uint8_t GetPeakFrequency(int16_t* freqArray);
};

