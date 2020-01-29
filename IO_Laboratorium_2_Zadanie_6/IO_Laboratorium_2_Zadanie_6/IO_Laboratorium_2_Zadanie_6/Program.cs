using System;
using System.IO;
using System.Text;
using System.Threading;

namespace IO_Laboratorium_2_Zadanie_6
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] buffer = new byte[2048];

            FileStream fs = new FileStream("wejscie_lab2.txt", FileMode.Open, FileAccess.Read);
            AutoResetEvent are = new AutoResetEvent(false);
            fs.BeginRead(buffer, 0, buffer.Length, myAsyncCallback, new object[] { fs, buffer, are });
            are.WaitOne();
            Console.WriteLine("\nZakończenie pracy wątku głównego!");
        }

        static void myAsyncCallback(IAsyncResult result)
        {
            object[] values = (object[])result.AsyncState;
            FileStream fs = (FileStream)values[0];
            byte[] buffer = (byte[])values[1];
            AutoResetEvent are = (AutoResetEvent)values[2];
            Console.WriteLine("Treść pliku tekstowego:\n\n" + new UTF8Encoding().GetString(buffer));
            fs.Close();
            are.Set();
        }
    }
}
