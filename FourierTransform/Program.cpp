#include <iostream>
#include <cstdint>
#include "SignalGenerator.h"

int main()
{
    uint16_t* signal;
    // can I do this in one line?
    signal = SignalGenerator::GetWaveForm(1000);

    for (int i = 0; i < 64; i++)
    {
        std::cout << signal[i];
        std::cout << "\n";
    }
}