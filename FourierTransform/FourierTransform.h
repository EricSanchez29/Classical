#pragma once

#define _USE_MATH_DEFINES

#include <cstdint>;
#include <cmath>;

class FourierTransform
{
    public:
        static int16_t* FFT(uint16_t* inputArray);
        static int32_t* DFT(uint16_t* inputArray);// messed around with different sized implementations, this didn't fit on the arduino
        static uint8_t GetPeakFrequency(int32_t* freqArray);


        static double* FFT(double* input_TimeSpace);
        static double* DFT(double* input_TimeSpace);

        static int GetPeakFrequency(double* freqArray);
        // I want to know how many overtones are present
        // to further analyze the accuracy of Transform
        static int* GetOrderedFrequencies(double* freqArray);

    private:
        static int16_t* fft_helper(uint16_t* input);
};

