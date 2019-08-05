using System.Collections.Generic;

namespace NetworkLib
{
    internal abstract class BaseChannel
    {
        protected readonly Player Peer;
        protected readonly Queue<NetPacket> OutgoingQueue;

        protected BaseChannel(Player peer)
        {
            Peer = peer;
            OutgoingQueue = new Queue<NetPacket>(64);
        }

        public int PacketsInQueue
        {
            get { return OutgoingQueue.Count; }
        }

        public void AddToQueue(NetPacket packet)
        {
            lock (OutgoingQueue)
            {
                OutgoingQueue.Enqueue(packet);
            }
        }

        public abstract void SendNextPackets();
        public abstract void ProcessPacket(NetPacket packet);
    }
}
