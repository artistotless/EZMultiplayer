using System.Collections.Generic;


namespace NetworkLib
{
   public class RoomData
    {
        public uint playersCount; // Текущее кол-во игроков
        public bool isPrivate // Запаролен?
        {
            get
            {
                return string.IsNullOrEmpty(password) ? isPrivate : true;
            }
            private set { }
        }

        public uint maxPlayers; // Максимальное кол-во игроков
        public string hostName; // Тот,кто создал комнату
        public string name; // Название комнаты
        public string password { get; private set; } // Пароль комнаты

        public Dictionary<string, string> roomData = new Dictionary<string, string>(); // Дополнительные параметры комнаты

        private RoomData() { }

        public RoomData(string Name, string Host, uint MaxPlayers, uint PlayersCount, bool IsPrivate)
        {
            name = Name;
            hostName = Host;
            maxPlayers = MaxPlayers;
            playersCount = PlayersCount;
            isPrivate = IsPrivate;
        }
        public RoomData(string Name, string Host, uint MaxPlayers, string password)
        {
            name = Name;
            hostName = Host;
            maxPlayers = MaxPlayers;
            this.password = password;

        }
    }
}
