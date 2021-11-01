using System;
using System.Net;
using System.Threading;
using ServerCore;

namespace Server
{
    class Program
    {
        static readonly List listener = new();
        public static GameRoom Room = new GameRoom();

        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new(ipAddr, 7777);

            listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
            Console.WriteLine("Listening...");

            while (true)
            {
                Room.Push(() => Room.Flush());
                Thread.Sleep(250);
            }
        }
    }
}
