#define _USE_MATH_DEFINES
#include <cstdint>
#include <cmath>;

class SignalGenerator
{
	public:

		static uint16_t* GetPositiveWaveForm(uint16_t cycles, double amplitude);

		static double* GetCosine(int cycles, double amplitude);

		static double* AddWaves(double* wave1, double* wave2);
};

