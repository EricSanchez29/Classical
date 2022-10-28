#define _USE_MATH_DEFINES
#include <cstdint>
#include <cmath>;

class SignalGenerator
{
	public:

		static uint16_t* GetPositiveWaveForm(uint16_t cycles, double amplitude);

		static int16_t* GetWaveForm(uint16_t cycles, double amplitude);
};

