using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiteNetLib
{
    public class Room
    {
        public List<Player> players = new List<Player>();
        public uint playersCount { get { return (uint)(players.Count); } }

        public string name;
        private Dictionary<string, string> roomData = new Dictionary<string, string>();


        internal Room(string name, Dictionary<string, string> roomData)
        {
            this.name = name;
            this.roomData = roomData;
        }

        internal Room(string name)
        {
            this.name = name;
            this.roomData = null;
        }

        internal void Broadcast(byte[] data)
        {
            foreach (Player player in players)
            {
                player.Send(data, DeliveryMethod.ReliableOrdered);
            }
        }

        internal Player[] GetPlayers()
        {
            if (players != null || playersCount != 0)
            {
                return players.ToArray();
            }
            return null;
        }
    }
}
