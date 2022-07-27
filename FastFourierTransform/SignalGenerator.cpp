#include "SignalGenerator.h"
#include <cmath>;
//#include <math>;

// freq is relative to T, time between "sampled" points in the waveform array
// 1 "Hz" = 1 / T
uint8_t* SignalGenerator::GetWaveForm(uint16_t freq)
{
	// how can I make a variable length array?

	uint8_t static waveform[64];

	// chose 15 digits after decimal
// similar to rounding limit in C#, but ultimately this is arbitrary
	double _pi = 3.141592653589793;
	
	// analog signal on an Arduino has 10 bits of resolution
	// so a 5 volt signal will be 1023 0000 0011 1111 1111
	// and a 0 volt signal will be 0   0000 0000 0000 0000


	// only go through one full cycle

	// sin(0) = 0
	// sin(pi/2) = 1

	waveform[0] = 0;

	double wave[64];

	wave[0] = 0;

	for (int n = 1; n < 64; n++)
	{
		double num = round((1024) * sin((2*_pi * n) / (64)) + 1024);

		waveform[n] = (uint8_t)num;
		
		wave[n] = num;
	}

	return waveform;
}
