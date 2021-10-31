using ServerCore;
using System;
using System.Collections.Generic;

class PacketManager
{
    #region Singleton
    static readonly PacketManager instance = new();
    public static PacketManager Instance { get { return instance; } }
    #endregion

    PacketManager()
    {
        Register();
    }

    readonly Dictionary<ushort, Action<PacketSession, ArraySegment<byte>>> onRecv = new();
    readonly Dictionary<ushort, Action<PacketSession, IPacket>> handler = new();

    public void Register()
    {
        onRecv.Add((ushort)PacketID.C_Chat, MakePacket<C_Chat>);
        handler.Add((ushort)PacketID.C_Chat, PacketHandler.C_ChatHandler);

    }

    public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
    {
        ushort count = 0;

        ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
        count += 2;
        ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
        count += 2;

        if (onRecv.TryGetValue(id, out Action<PacketSession, ArraySegment<byte>> action))
            action.Invoke(session, buffer);
    }

    void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
    {
        T pkt = new();
        pkt.Read(buffer);
        if (handler.TryGetValue(pkt.Protocol, out Action<PacketSession, IPacket> action))
            action.Invoke(session, pkt);
    }
}