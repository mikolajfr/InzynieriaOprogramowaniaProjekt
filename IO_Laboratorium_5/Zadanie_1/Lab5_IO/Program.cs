using System;

namespace Lab5_IO
{
    class Program
    {
        static void Main(string[] args)
        {
            // UWAGA - PLIK NIE DO URUCHOOMIENIA - ZOSTAŁ NAPISANY W JĘZYKU C
            float* tmp = (float*)malloc(sizeof(float) * w * h);
            for (int n = 0; n < iter; n++)
            {
                for (int y = 0; y < n; y++)
                {
                    for (int x = 0; x < w; x++)
                    {
                        int index = y * w + x;
                        tmp[index] = hx[4] * img[index];
                        if (x > 0) tmp[index] += hx[3] * img[index - 1];
                    }
                }
                img = tmp;
            }
        }
    }
}
