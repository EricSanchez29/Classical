#include "FourierTransform.h"
#include <cmath>;

double Pi = 3.141592653589793;

int16_t* FourierTransform::GetDFT(uint16_t* inputArray)
{
	int16_t static real_result[128];

	// might delete later
	// will just use to compare with FFT
	int16_t imag_result[128];

	for (int k = 0; k < 128; k++)
	{
		int16_t real_value = 0;

		int16_t imag_value = 0;

		for (int m = 0; m < 128; m++)
		{
			real_value = real_value + round(inputArray[m] * cos((2 * Pi * m * k) / 128));
			imag_value = imag_value + round(inputArray[m] * sin((2 * Pi * m * k) / 128));
		}

		real_result[k] = real_value;
		imag_result[k] = imag_value;
	}

	return real_result;
}

int16_t* FourierTransform::GetFFT(uint16_t* inputArray)
{
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

	return nullptr;
}
