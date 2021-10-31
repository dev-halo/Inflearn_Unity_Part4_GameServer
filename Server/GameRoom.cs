using System;
using System.Collections.Generic;

namespace Server
{
    class GameRoom
    {
        readonly List<ClientSession> sessions = new();
        readonly object _lock = new();

        public void Broadcast(ClientSession session, string chat)
        {
            S_Chat packet = new()
            {
                playerId = session.SessionId,
            };
            packet.chat = $"{chat} I am {packet.playerId}";
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
