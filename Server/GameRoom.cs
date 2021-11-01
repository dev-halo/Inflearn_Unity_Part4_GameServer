using ServerCore;
using System;
using System.Collections.Generic;

namespace Server
{
    class GameRoom : IJobQueue
    {
        readonly List<ClientSession> sessions = new();
        readonly JobQueue jobQueue = new();

        public void Push(Action job)
        {
            jobQueue.Push(job);
        }

        public void Broadcast(ClientSession session, string chat)
        {
            S_Chat packet = new()
            {
                playerId = session.SessionId,
            };
            packet.chat = $"{chat} I am {packet.playerId}";
            ArraySegment<byte> segment = packet.Write();

            foreach (ClientSession s in sessions)
                s.Send(segment);
        }

        public void Enter(ClientSession session)
        {
            sessions.Add(session);
            session.Room = this;
        }

        public void Leave(ClientSession session)
        {
            sessions.Remove(session);
        }
    }
}
