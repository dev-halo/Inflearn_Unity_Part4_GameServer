﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using ServerCore;

namespace Server
{
    class Program
    {
        static List listener = new List();

        static void Main(string[] args)
        {
            PacketManager.Instance.Register();

            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            listener.Init(endPoint, () => { return new ClientSession(); });
            Console.WriteLine("Listening...");

            while (true)
            {
            }
        }
    }
}
