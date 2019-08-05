using NetworkLib;
using System;
using System.Threading;


namespace EZMultiplayer_Server_1._0._0
{
    class Program
    {
        private static Listener _serverListener;
        private const int Port = 9050; // Порт сервера | Server port
        static void Main(string[] args)
        {
            Console.WriteLine("*** EZMutiplayer Server by unity3ddd.ru ***");

            EZMultiplayer.StartServer(Port, out _serverListener);
           
            while (!Console.KeyAvailable)
            {
                EZMultiplayer.ReceiveMessage();
                Thread.Sleep(15);
            }
         
            EZMultiplayer.StopServer();

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();

        }
    }
}
