using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace IO_Laboratorium_1_Zadanie_5
{
    class Program
    {
        public readonly static object SumLock = new object();
        public const int rozmiarTablicy = 512;
        public const int rozmiarBloku = 128;
        public static int suma = 0;
        public static int[] tablica_wartosci = new int[rozmiarTablicy];


        static void Main(string[] args)
        {
            // Losowanie wartosci do tablicy
            Random random_generator = new Random();
            for(int i = 0; i < rozmiarTablicy; i++)
            {
                tablica_wartosci[i] = random_generator.Next(1, 2000);
            }

            // Generowanie tablicy AutoResetEvent oraz ustawienie jej pól na false
            // Wątek zostaje dodany do kolejki i następuje rozpoczęcie operacji sumowania
            AutoResetEvent[] are_tab = new AutoResetEvent[rozmiarTablicy / rozmiarBloku];
            for(int i=0; i < are_tab.Length; i++)
            {
                are_tab[i] = new AutoResetEvent(false);
                ThreadPool.QueueUserWorkItem(Sumowanie, new object[]{ i, are_tab[i] });
            }

            // Oczekiwanie na zakończenie operacji sumowania przez wszystkie wątki
            WaitHandle.WaitAll(are_tab);
            Console.WriteLine("Zakończono operację sumowania!\n");
            Console.WriteLine("Uzyskany wielowątkowo wynik: {0}", suma);

            // Kontrolna suma jednowątkowa
            int suma_tab = 0;
            for (int i = 0; i < tablica_wartosci.Length; i++)
                suma_tab += tablica_wartosci[i];

            Console.WriteLine("Uzyskany jednowątkowo wynik: {0}\n", suma_tab);

            if (suma == suma_tab)
                Console.WriteLine("Uzyskane sumy są identyczne!");
            else
                Console.WriteLine("Uzyskane sumy są od siebie różne!");
        }
        
        static void Sumowanie(Object StateInfo)
        {
            int indeks = (int)((object[])StateInfo)[0];
            AutoResetEvent are = (AutoResetEvent)((object[])StateInfo)[1];
            int indeks_poczatkowy = rozmiarBloku * indeks;
            int indeks_koncowy = rozmiarBloku * (indeks + 1) - 1;

            // Sumowanie wartości wyznaczonych dla wątku indeksów tablicy
            int pomocnicza_suma = 0;
            lock(SumLock)
            {
                for(int i = indeks_poczatkowy; i <= indeks_koncowy; i++)
                {
                    pomocnicza_suma += tablica_wartosci[i];
                }
            }
            suma += pomocnicza_suma;
            are.Set();
        }

    }
}
