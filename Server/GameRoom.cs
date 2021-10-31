using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class GameRoom
    {
        List<ClientSession> sessions = new List<ClientSession>();
        object _lock = new();

        public void Broadcast(ClientSession session, string chat)
        {
            S_Chat packet = new()
            {
                playerId = session.SessionId,
                chat = chat
            };
            ArraySegment<byte> segment = packet.Write();

            lock (_lock)
            {
                foreach (ClientSession s in sessions)
                    s.Send(segment);
            }
        }

        public void Enter(ClientSession session)
        {
            lock (_lock)
            {
                sessions.Add(session);
                session.Room = this;
            }
        }

        public void Leave(ClientSession session)
        {
            lock (_lock)
            {
                sessions.Remove(session);
            }
        }
    }
}
