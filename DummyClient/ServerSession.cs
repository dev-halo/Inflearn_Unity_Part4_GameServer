using ServerCore;
using System;
using System.Net;
using System.Text;

namespace DummyClient
{
    class ServerSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            C_PlayerInfoReq packet = new C_PlayerInfoReq() { playerId = 1001, name = "Halo" };
            var skill = new C_PlayerInfoReq.Skill() { id = 101, level = 1, duration = 3f };
            skill.attributes.Add(new C_PlayerInfoReq.Skill.Attribute() { att = 77 });
            packet.skills.Add(skill);
            packet.skills.Add(new C_PlayerInfoReq.Skill() { id = 201, level = 2, duration = 4f });
            packet.skills.Add(new C_PlayerInfoReq.Skill() { id = 301, level = 3, duration = 5f });
            packet.skills.Add(new C_PlayerInfoReq.Skill() { id = 401, level = 4, duration = 6f });

            // 보낸다.
            ArraySegment<byte> s = packet.Write();
            if (s != null)
                Send(s);
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override int OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Server] {recvData}");
            return buffer.Count;
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes : {numOfBytes}");
        }
    }
}
