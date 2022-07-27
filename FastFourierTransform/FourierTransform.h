#pragma once
#include <cstdint>

/*
    Radix 2 FFT

    Xk (DFT) of a n point sequence x_m with N = 2^t

    First step of FFT is

    N1 = 2 and N2 = 2^(t-1)

    Xk = Sum(m = 0; N/2 - 1)
         {
            (x_2m * W^2mk) + W^k * Sum{
                                        x_2m+1 * W^2mk
                                      }
         }


    
*/
static class FourierTransform
{
    public:
        uint8_t* GetFFT(uint8_t* inputArray);
        uint8_t* GetDFT(uint8_t* inputArray);

    private:

};

