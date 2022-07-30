#include "SignalGenerator.h"
#include <cmath>;

// freq is relative to T, time between "sampled" points in the waveform array
// 1 "Hz" = 1 / T
uint16_t* SignalGenerator::GetWaveForm(uint16_t cycles, double amplitude)
{
	uint16_t static waveform[128];

	double Pi = 3.141592653589793;
	
	// analog signal on an Arduino has 10 bits of resolution
	// so a 5 volt signal will be 1023 0000 0011 1111 1111
	// and a 0 volt signal will be 0   0000 0000 0000 0000

	// thats why I use the uint16_t type

	uint8_t offset = 0;
	 
	uint8_t cycleLength = 128 / cycles;

	for (uint8_t i = 1; i <= cycles; i++)
	{
		for (uint8_t n = 1; n <= cycleLength; n++)
		{
			int num = round((amplitude) * sin((2 * Pi * n) / cycleLength) + amplitude);

			waveform[n-1 + offset] = num;
		}

		offset = offset + cycleLength;
	}

	return waveform;
}
