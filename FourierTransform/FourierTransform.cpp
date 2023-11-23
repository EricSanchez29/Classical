#include "FourierTransform.h"
#include <map>
#include <iostream>
#include <vector>
#include <algorithm>

int32_t* FourierTransform::DFT(uint16_t* inputArray)
{
	int32_t static real_result[256];

	// might delete later
	// will just use to compare with FFT
	//int16_t imag_result[256];

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

double* FourierTransform::DFT(double* input_TimeSpace)
{
	double static realResult[256];

	double imag_result[256];

	for (uint16_t k = 0; k < 256; k++)
	{
		float realValue = 0;

		float imagValue = 0;

		for (uint16_t m = 0; m < 256; m++)
		{
			realValue = realValue + input_TimeSpace[m] * cos((2 * M_PI * m * k) / 256);
			imagValue = imagValue + input_TimeSpace[m] * sin((2 * M_PI * m * k) / 256);
		}

		realResult[k] = realValue;
		imag_result[k] = imagValue;
	}

	return realResult;
}

int FourierTransform::GetPeakFrequency(double* freqArray)
{
	uint8_t peakFreq = 0;

	// finds the first peak frequency, might discover multiple peaks in the process, 
	// return the smaller frequency (larger ones are possible overtones)
	double peakValue = freqArray[0];

	int peakCount = 0;

	// find largest value starting from index 0, ignore subsequent equivalent values (repeat peaks)
	for (uint16_t i = 1; i < 256; i++)
	{
		double currentValue = abs(freqArray[i]);

		if (peakValue < currentValue)
		{
			peakValue = currentValue;
			peakFreq = i;
		}
	}

	return peakFreq;
}

bool cmp(std::pair<int, double>& a,
	std::pair<int, double>& b)
{
	return a.second < b.second;
}

int* FourierTransform::GetOrderedFrequencies(double* freqArray)
{
	int static resultArray[256];

	// 256 element array
	// return array of indexes for biggest to smallest array value

	//naive implementation
	// find biggest value by looping through entire array
	// find next biggest value by looping through entire list again
	// ... 
	// O(n^n)

	// better to make a vector pair
	// populate it with index/value pairs
	// sort by value, largest to smallest

	std::vector<std::pair<int, double> > vectorPair;
	
	// populate
	for (int i = 0; i < 256; i++)
	{
		vectorPair.push_back(std::make_pair(i, freqArray[i]));
	}

	// sort in reverse order, will access last element and pop_back
	std::sort(vectorPair.begin(), vectorPair.end(), cmp);

	// print, remove later
	/*for (auto& it : vectorPair) {
		std::cout << it.first << "->" << it.second << std::endl;
	}*/
	
	// create array
	for (int i = 0; i < 256; i++)
	{
		resultArray[i] = vectorPair.back().first;
		vectorPair.pop_back();
	}

	return resultArray;
}

double* FourierTransform::FFT(double* input_TimeSpace)
{
	return nullptr;
}

uint8_t FourierTransform::GetPeakFrequency(int32_t* freqSpace)
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

	// Based on the number of complex multiplications and complex additions
	//  O(N) = N log_2 N
	// where N is the length of the input array




	return nullptr;
}


int16_t* FourierTransform::fft_helper(uint16_t* inputArray)
{
	return nullptr;
}