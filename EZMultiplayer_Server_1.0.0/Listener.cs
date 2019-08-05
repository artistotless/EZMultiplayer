using System;
using System.Net;

using System.Collections.Generic;
using NetworkLib;
using NetworkLib.Utils;

namespace EZMultiplayer_Server_1._0._0
{
    public class Listener : INetEventListener
    {
        internal List<Player> _connectedPlayers = new List<Player>();
        internal List<Room> _rooms = new List<Room>();

        #region UserEvents 
        //События,связанные с игроком

        public void OnUserJoined(Player player)
        {
            _connectedPlayers.Add(player);
            Console.WriteLine("[Server] player connected: " + player.name);
        }

        public void OnUserLeft(Player player, DisconnectInfo disconnectInfo)
        {
            _connectedPlayers.Remove(player);
            Console.WriteLine("[Server] player disconnected: " + player.name + ", reason: " + disconnectInfo.Reason);
        }


        public void OnGotMessage(Player player, NetDataReader reader, DeliveryMethod deliveryMethod)
        {
            //echo
            //Console.WriteLine("[Server] player@" + player.name + ": " + reader.GetString());

            switch (reader.Type)
            {
                case "MoveTank":

                    var msg = new Message();

                    msg.Type("MoveTank");
                    msg.Add(player.name);
                    msg.Add(reader.GetFloat());
                    msg.Add(reader.GetFloat());

                    player.CurrentRoom().Broadcast(msg);
                    break;

                case "ShootTank":

                    var shootMSG = new Message();

                    shootMSG.Type("ShootTank");
                    shootMSG.Add(player.name);
                    shootMSG.Add(reader.GetFloat());
                    shootMSG.Add(reader.GetFloat());

                    player.CurrentRoom().Broadcast(shootMSG);
                    break;
                case "RotateTank":

                    var rotateMSG = new Message();

                    rotateMSG.Type("RotateTank");
                    rotateMSG.Add(player.name);
                    rotateMSG.Add(reader.GetFloat());
                    rotateMSG.Add(reader.GetFloat());

                    player.CurrentRoom().Broadcast(rotateMSG);
                    break;
                case "ChatOfRoom":

                    var chatMsg = new Message();

                    chatMsg.Type("ChatOfRoom");
                    chatMsg.Add(player.name);
                    chatMsg.Add(reader.GetString());
                    player.CurrentRoom().Broadcast(chatMsg);
                    break;

            }

        }


        public void OnNetworkLatencyUpdate(Player player, int latency)
        {

        }

        public void OnConnectionRequest(ConnectionRequest request)
        {
            request.Accept().name = request.Data.GetString();
            Console.WriteLine(
                "[Server] ConnectionRequest. Ep: {0}, Accepted: {1}",
                request.RemoteEndPoint,
                true);
        }
        #endregion

        #region RoomEvents 
        //События,связанные с комнатами

        public void OnUserCreateRoom(Room room, Player player)
        {
            if (room != null) // Success
            {
                _rooms.Add(room);
                Console.WriteLine("[Server] player#{0} created Room [{1}] ", player.name, room.name);
                //var msg = new Message();
                //msg.Type("Hello");
                //msg.Add("ddd");
                //EZMultiplayer.BroadCastToRoom(player, msg, false);
            }
            else // Error
            {

            }

        }

        public void OnUserJoinToRoom(Room room, Player player)
        {
            if (room != null)// Success
            {
                Console.WriteLine("[Server] player#{0} join to Room [{1}] ", player.name, room.name);
            }
            else // Error
            {

            }
        }

        public void OnUserLeftRoom(Room room, Player player)
        {
            Console.WriteLine("[Server] player#{0} left Room [{1}] ", player.name, room.name);
        }

        public void OnStartedGame(Room room)
        {
            if (room != null)// Success
            {

            }
            else // Error
            {

            }
        }

        public void OnDeleteRoom(Room room)
        {
            _rooms.Remove(room);
            Console.WriteLine("[Server] Room [{0}] was deleted  ", room.name);

        }



        #endregion

        #region GeneralEvents
        //Общие события

        public void OnNetworkError(IPEndPoint endPoint, int socketErrorCode)
        {
            Console.WriteLine("[Server] error: " + socketErrorCode);
        }

        public void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetDataReader reader, UnconnectedMessageType messageType)
        {
            Console.WriteLine("[Server] ReceiveUnconnected: {0}", reader.GetString(100));
        }


        #endregion
    }



}