using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace IO_Laboratorium_1_Zadanie_2
{
    // Problemy:
    // a) Bez podania odpowiednich opóźnień wątki wywołane wcześniej niekoniecznie są też wcześniej wykonywane
    // b) Brak obsługi zerwania połączenia z jednym z kilentów
    // c) W razie opóźnień w transmisji teoretycznie może dojść do sytaucji, w której wątek główny zakończy
    // połączenie zanim nastąpi pełna transmisja danych między klientem, a serwerm
    // d) Najpierw obsłużona zostaje transmisja z jednym klientem (wiadmość od klienta oraz zwrotna od serwera)
    // a dopiero później transmisja z 2 klientem
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool.QueueUserWorkItem(ThreadProc_server);
            ThreadPool.QueueUserWorkItem(ThreadProc_client, new object[] { 1, "Witaj!" });
            ThreadPool.QueueUserWorkItem(ThreadProc_client, new object[] { 2, "Czesc!" });
            Thread.Sleep(4000);
            Console.WriteLine("Koniec działania programu");
        }
        static void ThreadProc_server(Object stateInfo)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();

            while(true)
            {
                byte[] echo_str = new byte[1024];
                echo_str = new ASCIIEncoding().GetBytes("ECHO");
                TcpClient client = server.AcceptTcpClient();
                byte[] buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, 1024);
                Console.WriteLine(new ASCIIEncoding().GetString(buffer));
                client.GetStream().Write(echo_str, 0, echo_str.Length);
                client.Close();
            }
        }

        static void ThreadProc_client(Object stateInfo)
        {
            var nr_klienta = ((object[])stateInfo)[0];
            var wiadomosc = ((object[])stateInfo)[1];
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            byte[] message = new ASCIIEncoding().GetBytes("Klient " + (int)nr_klienta + ": " + (String)wiadomosc);
            client.GetStream().Write(message, 0, message.Length);
            NetworkStream stream = client.GetStream();
            message = new byte[1024];
            stream.Read(message, 0, message.Length);
            Console.WriteLine(new ASCIIEncoding().GetString(message));
        }
    }
}
