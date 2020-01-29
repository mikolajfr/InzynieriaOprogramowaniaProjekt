using System;

namespace IO_Laboratorium_2_Zadanie_8
{
    class Program
    {
        delegate int DelegateType(object argument);
        static DelegateType delegate_silniaRek;
        static DelegateType delegate_silniaIt;
        static DelegateType delegate_fibbonaciRek;
        static DelegateType delegate_fibbonaciIt;

        static int SilniaIt(object argument)
        {
            int wynik = 1;

            if ((int)argument == 0)
                return 0;

            for (int i = 1; i <= (int)argument; i++)
            {
                wynik *= i;
            }
            return wynik;
        }

        static int FibbonaciIt(object argument)
        {
            int poprzedni = 1;
            int nastepny = 1;
            if ((int)argument < 2)
                return 1;

            for (int i = 2; i < (int)argument; i++)
            {
                int temp = nastepny;
                nastepny += poprzedni;
                poprzedni = temp;
            }
            return nastepny;
        }

        static int SilniaRek(object argument)
        {
            if ((int)argument < 2)
                return 1;
            return (int)argument * SilniaRek((int)argument - 1);
        }

        static int FibbonaciRek(object argument)
        {
            if ((int)argument == 0)
                return 0;
            if ((int)argument == 1 || (int)argument == 2)
                return 1;

            return FibbonaciRek((int)argument - 1) + FibbonaciRek((int)argument - 2);
        }

        static void SilniaRekCallback(IAsyncResult ia_result)
        {
            Console.WriteLine("Obliczono wartość silni metodą rekurencyjną");
        }

        static void SilniaItCallback(IAsyncResult ia_result)
        {
            Console.WriteLine("Obliczono wartość silni metodą iteracyjną");
        }

        static void FibbonaciRekCallback(IAsyncResult ia_result)
        {
            Console.WriteLine("Rekurencyjnie obliczono wartość elementu ciągu Fibbonaciego");
        }

        static void FibbonaciItCallback(IAsyncResult ia_result)
        {
            Console.WriteLine("Iteracyjnie obliczono wartość elementu ciągu Fibbonaciego");
        }

        static void Main(string[] args)
        {
            delegate_silniaRek = new DelegateType(SilniaRek);
            IAsyncResult iar_silRek = delegate_silniaRek.BeginInvoke(10, new AsyncCallback(SilniaRekCallback), null);
            int wynik_silnia_rek = delegate_silniaRek.EndInvoke(iar_silRek);

            delegate_silniaIt = new DelegateType(SilniaIt);
            IAsyncResult iar_silIt = delegate_silniaIt.BeginInvoke(10, new AsyncCallback(SilniaItCallback), null);
            int wynik_silnia_it = delegate_silniaIt.EndInvoke(iar_silIt);

            delegate_fibbonaciRek = new DelegateType(FibbonaciRek);
            IAsyncResult iar_fibbonaciRek = delegate_fibbonaciRek.BeginInvoke(20, new AsyncCallback(FibbonaciRekCallback), null);
            int wynik_fibbonaciRek = delegate_fibbonaciRek.EndInvoke(iar_fibbonaciRek);

            delegate_fibbonaciIt = new DelegateType(FibbonaciIt);
            IAsyncResult iar_fibbonaciIt = delegate_fibbonaciIt.BeginInvoke(20, new AsyncCallback(FibbonaciItCallback), null);
            int wynik_fibbonaciIt = delegate_fibbonaciIt.EndInvoke(iar_fibbonaciIt);

            Console.WriteLine("Wynik rekurencyjnie obliczonej silni: " + wynik_silnia_rek);
            Console.WriteLine("Wynik iteracyjnie obliczonej silni: " + wynik_silnia_it);
            Console.WriteLine("Wartosc 20 elementu ciagu fibbonaciego (rekurencyjnie): " + wynik_fibbonaciRek);
            Console.WriteLine("Wartosc 20 elementu ciagu fibbonaciego (iteracyjnie): " + wynik_fibbonaciIt);
            Console.ReadKey();
        }
    }
}
