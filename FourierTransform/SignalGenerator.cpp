#include "SignalGenerator.h"

// Returns 256 length array of uint16_t representing a sinusoid of positive values
// freq is relative to T, time between "sampled" points in the waveform array
// 1 "Hz" = 1 / T
uint16_t* SignalGenerator::GetPositiveWaveForm(uint16_t cycles, double amplitude)
{
	uint16_t static waveform[256];

	// analog signal on an Arduino has 10 bits of resolution
	// so a 5 volt signal will be 1023 0000 0011 1111 1111
	// and a 0 volt signal will be 0   0000 0000 0000 0000

	// thats why I use the uint16_t type

	// can also use 12 bits on a Arduino Due and Zero
	// 0 - 4095
	
	// for waveform index
	uint8_t offset = 0;

	// freq == 1 / (cyclelength * Time between measurements)
	uint16_t cycleLength = 256 / cycles;

	for (uint16_t i = 1; i <= cycles; i++)
	{
		for (uint16_t n = 1; n <= cycleLength; n++)
		{
			int16_t num = round((amplitude) * sin((2 * M_PI * n) / cycleLength) + amplitude);

			waveform[n-1 + offset] = num;
		}

		offset = offset + cycleLength;
	}

	return waveform;
}

int16_t* SignalGenerator::GetWaveForm(uint16_t cycles, double amplitude)
{
	return nullptr;
}
