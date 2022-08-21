#include <fstream>
#include <iostream>
#include <cstdint>
#include "SignalGenerator.h"
#include "FourierTransform.h"
using namespace std;

int main()
{

    for (uint8_t i = 1; i <= 2; i++)
    {
        //uint16_t* signal;




        // can I do this in one line?
        //signal = SignalGenerator::GetPositiveWaveForm(i, 511.5);

        uint16_t j = 0;

        

        char fileLocation[13] = "C_sample.txt";
            

        std::fstream testFile;


        testFile.open(fileLocation, std::fstream::in);

        uint16_t value;

        j = 0;


        uint16_t signal[256];

        //// read in file
        if (testFile.is_open())
        {   
            while (testFile >> value)
            {
                std::cout << value;
                std::cout << "\n";

                signal[j] = value;
            }
         
            j++;
        }

        testFile.close();

        /*for (j = 0; j < 256; j++)
        {
            std::cout << signal[j];
            std::cout << ",";
        }

        std::cout << "\n";*/


        int32_t* signal_freqSpace;

        signal_freqSpace = FourierTransform::DFT(signal);

        for (uint16_t i = 0; i < 256; i++)
        {
            std::cout << signal_freqSpace[i];
            std::cout << ",";
        }

        std::cout << "\n";

        int a = FourierTransform::GetPeakFrequency(signal_freqSpace);

        std::cout << a;
        std::cout << "\n";
    }
}