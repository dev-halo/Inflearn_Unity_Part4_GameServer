using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacketQueue
{
    public static PacketQueue Instance { get; } = new PacketQueue();

    readonly Queue<IPacket> packetQueue = new Queue<IPacket>();
    readonly object _lock = new object();

    public void Push(IPacket packet)
    {
        lock (_lock)
        {
            packetQueue.Enqueue(packet);
        }
    }

    public IPacket Pop()
    {
        lock (_lock)
        {
            if (packetQueue.Count == 0)
                return null;

            return packetQueue.Dequeue();
        }
    }
}
