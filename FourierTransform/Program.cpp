#include <iostream>
#include <cstdint>
#include "SignalGenerator.h"
#include "FourierTransform.h"

int main()
{
    uint16_t* signal;
    // can I do this in one line?
    signal = SignalGenerator::GetWaveForm(64, 511.5);

    for (int i = 0; i < 128; i++)
    {
        std::cout << signal[i];
        std::cout << ",";
    }

    std::cout << "\n";

    int16_t* signal_freqSpace;

    signal_freqSpace = FourierTransform::GetDFT(signal);

    for (int i = 0; i < 128; i++)
    {
        std::cout << signal_freqSpace[i];
        std::cout << ",";
    }

    std::cout << "\n";
}