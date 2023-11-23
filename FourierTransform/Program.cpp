#include <fstream>
#include <iostream>
#include <cstdint>
#include "SignalGenerator.h"
#include "FourierTransform.h"
#include "Helper.h"
using namespace std;

int main()
{

    for (uint8_t i = 1; i < 2; i++)
    {
        double* signal = SignalGenerator::GetCosine(4, 100);

        Helper::PrintArray(signal);

        //double* testSignal = SignalGenerator::GetCosine(4, 100);

        //signal = SignalGenerator::AddWaves(signal, testSignal);

        //Helper::PrintArray(signal);

        double* signalFreq = FourierTransform::DFT(signal);

        Helper::PrintArray(signalFreq);

        int peak = FourierTransform::GetPeakFrequency(signalFreq);

        std::cout << "\n";
        std::cout << "peak freq: ";
        std::cout << peak;
        std::cout << "\n";


        int* a = FourierTransform::GetOrderedFrequencies(signalFreq);
        Helper::PrintArray(a);


        //for (int j = 0; j < 256; j++)
        //{
        //    std::cout << signal[j];
        //    std::cout << ",";
        //}

        //std::cout << "\n";

        //int32_t* signal_freqSpace;

        //signal_freqSpace = FourierTransform::DFT(signal);

        //for (uint16_t k = 0; k < 256; k++)
        //{
        //    std::cout << signal_freqSpace[k];
        //    std::cout << ",";
        //}

        //std::cout << "\n";

        //int a = FourierTransform::GetPeakFrequency(signal_freqSpace);

        //std::cout << a;
        //std::cout << "\n";
    }
}