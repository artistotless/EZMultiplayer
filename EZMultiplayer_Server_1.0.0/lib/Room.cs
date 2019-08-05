using NetworkLib.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NetworkLib
{
    public class Room
    {
        public List<Player> players = new List<Player>();
        public uint playersCount { get { return (uint)(players.Count); } }
        public bool isPrivate = false;
        public string name { get; private set; }
        internal uint maxPlayers;
        internal string password;
        internal Dictionary<string, string> roomData;
        internal Player host;// Тот,кто создал комнату
        internal RoomState state = RoomState.WaitingPlayers;

        private string[] playerNames { get { return (from item in players select item.name).ToArray(); } }

        internal Room(RoomParameters parameters)
        {
            name = parameters.name;
            isPrivate = parameters.password == "0" ? false : true;
            password = parameters.password;
            maxPlayers = parameters.maxPlayers;
            roomData = parameters.data;
            host = parameters.host;
            AddPlayer(host, password);

        }



        internal void Broadcast(Message message)
        {
            for (int index = 0; index < players.Count; index++)
            {
                players[index].Send(message, DeliveryMethod.ReliableOrdered);
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

        //Может вызывать только host
        public bool KickPlayer(string kickPlayer, Player host)
        {
            if (this.host == host) // Если человек,который отправил запрос хост,то продолжаем
            {
                for (int i = players.Count - 1; i >= 0; --i)
                {
                    if (players[i].name == kickPlayer)
                    {
                        EZMultiplayer.instance.PlayerLeaveRoom(players[i]);
                        return true;
                    }
                }
            }
            return false;
        }
        public Player ContainPlayer(string name)
        {
            for (int i = players.Count - 1; i >= 0; --i)
            {
                if (players[i].name == name) { return players[i]; }
            }
            return null;
        }

        internal bool AddPlayer(Player player, string password)
        {
            if (state == RoomState.WaitingPlayers && playersCount < maxPlayers && !player.InRoom())
            {
                if (isPrivate)
                {
                    if (password != this.password) { return false; }
                    players.Add(player);
                    if (playersCount == maxPlayers && ServerConfig.autoStartGame) { StartGame(host); }
                    return true;
                }
                players.Add(player);
                if (playersCount == maxPlayers && ServerConfig.autoStartGame) { StartGame(host); }
                return true;

            }
            return false;
        }

        //Может вызывать только host или сервер
        private void UpdateInfoRoom(Player newHost, string newPassword, string newName, uint newMaxPlayers)
        {

            System.Console.WriteLine("UpdateInfo for room [" + name + "]");

            var msg = new Message();
            msg.Type("UpdateInfoRoom");
            try
            {
                host = newHost ?? host;

                msg.Add(host.name);
                msg.Add(password = newPassword ?? password);
                msg.Add(name = newName ?? name);
                msg.Add(maxPlayers = newMaxPlayers == 0 ? maxPlayers : newMaxPlayers);

            }
            catch { return; }
            //Если макс кол-во человек стало меньше чем исходное,то кикаем "лишних" людей
            if ((newMaxPlayers < this.playersCount) && (newMaxPlayers > 0))
            {
                for (int i = 0; i < this.maxPlayers; i++)
                {
                    KickPlayer(players[i].name, this.host);
                }
            }
            Broadcast(msg);
        }

        //Может вызывать только host или сервер
        internal void UpdateInfoRoom(RoomData newData)
        {
            UpdateInfoRoom(ContainPlayer(newData.hostName), newData.password, newData.name, newData.maxPlayers);

        }

        internal void StartGame(Player host)
        {

            if ((playersCount >= ServerConfig.minCountPlayersForStartGame) && (this.host == host))
            {
                state = RoomState.Started;


                if (GetPlayers() != null)
                {
                    var packetPlayers = new Message();
                    packetPlayers.Type("StartGame");
                    packetPlayers.AddArray(players.ToArray());
                    Broadcast(packetPlayers);

                }

            }

        }

        internal Room RemovePlayer(Player player)
        {
            if (player.CurrentRoom() == this)
            {

                if (player == host && playersCount > 1) { host = players[1]; UpdateInfoRoom(players[1], null, null, 0); } // Если хост ушел,то назначаем другого хоста
                players.Remove(player);
                player.SetRoom(null);
                if (playersCount < 1)
                {
                    state = RoomState.Closed;
                    //TODO:удаляем игру
                    return this;
                }
            }
            return null;
        }
    }
}
