#pragma once

#define _USE_MATH_DEFINES

#include <cstdint>;
#include <cmath>;

class FourierTransform
{
    public:
        static int16_t* FFT(uint16_t* inputArray);
        static int32_t* DFT(uint16_t* inputArray);// messing around with different sized implementations, this didn't fit on the arduino
        static uint8_t GetPeakFrequency(int32_t* freqArray);

    private:
        static int16_t* fft_helper(uint16_t* input);
};

