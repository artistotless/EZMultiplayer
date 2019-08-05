using NetworkLib;
using System.Collections.Generic;


namespace NetworkLib
{
    public class RoomParameters
    {
        internal string name, password;
        internal uint maxPlayers;
        internal Dictionary<string, string> data;
        internal Player host;

        public RoomParameters(string name, uint maxPlayers, string password, Dictionary<string, string> data, Player host)
        {
            this.name = name;
            this.password = password;
            this.data = data;
            this.maxPlayers = maxPlayers;
            this.host = host;
        }
    }
}
