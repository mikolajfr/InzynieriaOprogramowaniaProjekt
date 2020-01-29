using System;
using System.Threading;

namespace IO_Laboratorium_1_Zadanie_1
{
    class Program
    {
        // Wątki zostają dodane i wykonywane asynchronicznie.
        // Jeżeli wątek główny kończy swoją pracę, to wraz z nim pracę kończą wszystkie wątki poboczne.
        static void Main(string[] args)
        {
            // Dodanie 2 wątków do puli wątków Threadpool
            ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { 1500, 1 });
            ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { 1000, 2 });
            // Uśpienie wątku głównego
            Thread.Sleep(2000);
            Console.WriteLine("Zakończono działanie programu!");
        }

        static void ThreadProc(Object stateInfo)
        {
            var czas = ((object[])stateInfo)[0];
            var nr_watku = ((object[])stateInfo)[1];

            Thread.Sleep(500);
            Console.WriteLine("Wątek {0} poczekał {1} ms.", (int)nr_watku, (int)czas);
        }
    }
}
