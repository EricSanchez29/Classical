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

	//128 elements in freqArray

	int16_t peakValue = freqSpace[0];

	// keep track of avg value;
	//int16_t averageValue = peakValue;

	// is this actually worth doing?
	uint16_t i;

	// find largest value starting from index 0, ignore subsequent equivalent values (repeat peaks)
	for (i = 0; i < 256; i++)
	{
		uint16_t currentValue = abs(freqSpace[i]);

		if (peakValue < currentValue)
		{
			peakValue = currentValue;
			peakFreq = i;
			//averageValue = averageValue + currentValue;
		}
	}

	// Look at adjacent values to the peak value
	// see if we need to average a range of values to get real peak frequency (generally if the peak freq is not proportional 2^n)

	// search in both direction of the peak, look for if and when you cross total avg value line
	// (this is the current heurtistic I'm using, this is subject to change)


	// + is to the right of this peak aka 0
	// - is to the left of this peak aka 0
	
	//i = 1;
	//
	//while (freqSpace[peakFreq + i] > (3 * peakValue/4))
	//{
	//	if (peakFreq + i + 1 > 128)
	//	{
	//		break;
	//	}

	//	i++;
	//}

	//uint8_t rightBound = peakFreq + i - 1;

	//i = -1;

	//while (freqSpace[peakFreq + i] > (3 * peakValue / 4))
	//{
	//	if (peakFreq + i - 1 < 0)
	//	{
	//		break;
	//	}

	//	i--;
	//}

	//uint8_t leftBound = peakFreq + i + 1;

	//uint32_t sum = 0;

	//// take average of values in range from leftBound to rightBound to get approximate frequency
	//for (i = leftBound; i <= rightBound; i++)
	//{
	//	sum = sum + freqSpace[i];
	//}

	//sum = sum / (rightBound - leftBound + 1);

	//// approximate the real peak frequency
	//// which value is closest to the avg

	//for (i = 0; i < 128; i++)
	//{
	//	if ((freqSpace[i] < sum) && (sum < freqSpace[i + 1]))
	//	{
	//		// peak freq is somewhere between i and i+1
	//		// weighted avg?
	//	}
	//}



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
