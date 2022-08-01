#include <iostream>
#include <cstdint>
#include "SignalGenerator.h"
#include "FourierTransform.h"

int main()
{

    for (uint8_t i = 8; i < 32; i++)
    {



        uint16_t* signal;
        // can I do this in one line?
        signal = SignalGenerator::GetPositiveWaveForm(i, 511.5);

        /*for (uint8_t j = 0; j < 256; j++)
        {
            std::cout << signal[j];
            std::cout << ",";
        }

        std::cout << "\n";*/

        int16_t* signal_freqSpace;

        signal_freqSpace = FourierTransform::DFT(signal);

        /*for (uint8_t i = 0; i < 256; i++)
        {
            std::cout << signal_freqSpace[i];
            std::cout << ",";
        }

        std::cout << "\n";*/

        int a = FourierTransform::GetPeakFrequency(signal_freqSpace);

        std::cout << a;
        std::cout << "\n";
    }
}