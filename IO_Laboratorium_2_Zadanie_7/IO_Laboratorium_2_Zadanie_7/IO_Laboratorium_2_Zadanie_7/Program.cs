using System;
using System.IO;
using System.Text;
using System.Threading;

namespace IO_Laboratorium_2_Zadanie_7
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] buffer = new byte[2048];
            FileStream fs = new FileStream("wejscie_lab2.txt", FileMode.Open, FileAccess.Read);
            IAsyncResult ia_result = fs.BeginRead(buffer, 0, buffer.Length, null, new object[] { fs, buffer });
            fs.EndRead(ia_result);

            Console.WriteLine("Zakończono wczytywanie pliku tekstowego!");
            Console.WriteLine("Treść pliku tekstowego:\n\n" + new UTF8Encoding().GetString(buffer));

            Console.WriteLine("\nZakończenie pracy wątku głównego!");
        }
    }
}
