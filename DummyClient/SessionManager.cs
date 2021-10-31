using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DummyClient
{
    class SessionManager
    {
        static readonly SessionManager session = new();
        public static SessionManager Instance { get { return session; } }

        readonly List<ServerSession> sessions = new();
        readonly object _lock = new();

        public void SendForEach()
        {
            lock (_lock)
            {
                foreach (ServerSession session in sessions)
                {
                    C_Chat chatPacket = new()
                    {
                        chat = "Hello Server !"
                    };
                    ArraySegment<byte> segment = chatPacket.Write();

                    session.Send(segment);
                }
            }
        }

        public ServerSession Generate()
        {
            lock (_lock)
            {
                ServerSession session = new();
                sessions.Add(session);
                return session;
            }
        }
    }
}
