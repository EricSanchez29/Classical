#include "FourierTransform.h"

int16_t* FourierTransform::DFT(uint16_t* inputArray)
{
	int16_t static real_result[256];

	// might delete later
	// will just use to compare with FFT
	//int16_t imag_result[128];

	for (uint16_t k = 0; k < 256; k++)
	{
		float real_value = 0;

		//float imag_value = 0;

		for (uint16_t m = 0; m < 256; m++)
		{
			real_value = real_value + inputArray[m] * cos((2 * M_PI * m * k) / 256);
			//imag_value = imag_value + inputArray[m] * sin((2 * Pi * m * k) / 256);
		}

		real_result[k] = round(real_value);
		//imag_result[k] = round(imag_value);
	}

	return real_result;
}


uint8_t FourierTransform::GetPeakFrequency(int16_t* freqSpace)
{
	// gives frequency, relative to orignal sample rate in time space
	// aka the index of the freqArray
	uint8_t peakFreq = 0;

	// finds the first peak frequency, might discover multiple peaks in the process, 
	// return the smaller frequency (larger ones are possible overtones)
	int16_t peakValue = freqSpace[0];

	// find largest value starting from index 0, ignore subsequent equivalent values (repeat peaks)
	for (uint16_t i = 1; i < 256; i++)
	{
		int16_t currentValue = abs(freqSpace[i]);

		if (peakValue < currentValue)
		{
			peakValue = currentValue;
			peakFreq = i;
		}
	}

	return peakFreq;
}

int16_t* FourierTransform::FFT(uint16_t* inputArray)
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

// Need to do in next PR

	return nullptr;
}
