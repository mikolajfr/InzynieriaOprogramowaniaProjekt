using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace IO_Laboratorium_1_Zadanie_4
{
    // Wyrażenie lock pozwala na zablokowanie bloku instrukcji przez dany wątek celem zapobiegnięcia
    // sytuacji w której dochodzi do wzajemnego blokowania się przez nie.
    class Program
    {
        public readonly static object colorLock = new object();
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(ThreadProc_server);
            ThreadPool.QueueUserWorkItem(ThreadProc_client, new object[] { 1, "Witaj!" });
            ThreadPool.QueueUserWorkItem(ThreadProc_client, new object[] { 2, "Czesc!" });
            Thread.Sleep(3000);
            Console.WriteLine("Koniec działania programu");
        }
        static void ThreadProc_server(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(ThreadProc_connection, client);
            }
        }

        static void ThreadProc_connection(Object stateInfo)
        {
            byte[] echo_str = new byte[1024];
            echo_str = new ASCIIEncoding().GetBytes("ECHO");
            var client = (TcpClient)stateInfo;
            byte[] buffer = new byte[1024];
            client.GetStream().Read(buffer, 0, 1024);
            writeConsoleMessage("Otrzymałem wiadomość: " + new ASCIIEncoding().GetString(buffer), ConsoleColor.Green);
            client.GetStream().Write(echo_str, 0, echo_str.Length);
            client.Close();
        }

        static void ThreadProc_client(Object stateInfo)
        {
            var nr_klienta = ((object[])stateInfo)[0];
            var wiadomosc = ((object[])stateInfo)[1];
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            byte[] message = new ASCIIEncoding().GetBytes((String)wiadomosc);
            client.GetStream().Write(message, 0, message.Length);
            NetworkStream stream = client.GetStream();
            message = new byte[1024];
            stream.Read(message, 0, message.Length);
            writeConsoleMessage("Otrzymałem wiadomość: " + new ASCIIEncoding().GetString(message), ConsoleColor.Red);
        }

        static void writeConsoleMessage(string message, ConsoleColor color)
        {
            lock(colorLock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
    }
}
