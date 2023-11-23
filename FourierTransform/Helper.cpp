#include "Helper.h"

void Helper::PrintArray(double* array)
{
    std::cout << "\n";
    for (int i = 0; i < 256; i++)
    {
        std::cout << array[i];
        std::cout << ", ";
    }
    std::cout << "\n";
}
