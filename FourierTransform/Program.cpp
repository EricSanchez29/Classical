#include <fstream>
#include <iostream>
#include <cstdint>
#include "SignalGenerator.h"
#include "FourierTransform.h"
using namespace std;

int main()
{

    for (uint8_t i = 1; i <= 150; i++)
    {
        uint16_t* signal;
        // can I do this in one line?
        signal = SignalGenerator::GetPositiveWaveForm(i, 511.5);

        /*for (uint16_t j = 0; j < 128; j++)
        {
            std::cout << signal[j];
            std::cout << ",";
        }

        std::cout << "\n";*/

        //std::string fileLocation = "A_test.txt";

        //std::fstream testFile;


        //testFile.open(fileLocation, std::fstream::in);



        //// read in file
        //if (testFile.is_open())
        //{
        //    for (uint16_t i = 0; i < 256; i++)
        //    {
        //        if (testFile.Get getline(testFile, signal[i]))
        //        {
        //            break;
        //        }
        //    }
        //}

        //testFile.close();


        int32_t* signal_freqSpace;

        signal_freqSpace = FourierTransform::DFT(signal);

        /*for (uint16_t i = 0; i < 256; i++)
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