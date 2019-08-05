using System;
using System.Net;

using NetworkLib.Utils;

namespace NetworkLib
{
    /// <summary>
    /// Type of message that you receive in OnNetworkReceiveUnconnected event
    /// </summary>
    public enum UnconnectedMessageType
    {
        BasicMessage,
        DiscoveryRequest,
        DiscoveryResponse
    }

    /// <summary>
    /// Disconnect reason that you receive in OnPeerDisconnected event
    /// </summary>
    public enum DisconnectReason
    {
        SocketReceiveError,
        ConnectionFailed,
        Timeout,
        SocketSendError,
        RemoteConnectionClose,
        DisconnectPeerCalled,
        ConnectionRejected
    }

    /// <summary>
    /// Additional information about disconnection
    /// </summary>
    public struct DisconnectInfo
    {
        /// <summary>
        /// Additional info why peer disconnected
        /// </summary>
        public DisconnectReason Reason;

        /// <summary>
        /// Error code (if reason is SocketSendError or SocketReceiveError)
        /// </summary>
        public int SocketErrorCode;

        /// <summary>
        /// Additional data that can be accessed (only if reason is RemoteConnectionClose)
        /// </summary>
        public NetDataReader AdditionalData;
    }

    public interface INetEventListener
    {
        /// <summary>
        /// New remote peer connected to host, or client connected to remote host
        /// </summary>
        /// <param name="player">Connected peer object</param>
        void OnUserJoined(Player player);

        /// <summary>
        /// Player disconnected
        /// </summary>
        /// <param name="peer">disconnected peer</param>
        /// <param name="disconnectInfo">additional info about reason, errorCode or data received with disconnect message</param>
        void OnUserLeft(Player peer, DisconnectInfo disconnectInfo);

        /// <summary>
        /// Network error (on send or receive)
        /// </summary>
        /// <param name="endPoint">From endPoint (can be null)</param>
        /// <param name="socketErrorCode">Socket error code</param>
        void OnNetworkError(IPEndPoint endPoint, int socketErrorCode);

        /// <summary>
        /// Received some data
        /// </summary>
        /// <param name="peer">From peer</param>
        /// <param name="reader">DataReader containing all received data</param>
        /// <param name="deliveryMethod">Type of received packet</param>
        void OnGotMessage(Player peer, NetDataReader reader, DeliveryMethod deliveryMethod);

        /// <summary>
        /// Received unconnected message
        /// </summary>
        /// <param name="remoteEndPoint">From address (IP and Port)</param>
        /// <param name="reader">Message data</param>
        /// <param name="messageType">Message type (simple, discovery request or responce)</param>
        void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetDataReader reader, UnconnectedMessageType messageType);

        /// <summary>
        /// Latency information updated
        /// </summary>
        /// <param name="peer">Player with updated latency</param>
        /// <param name="latency">latency value in milliseconds</param>
        void OnNetworkLatencyUpdate(Player peer, int latency);

        /// <summary>
        /// On peer connection requested
        /// </summary>
        /// <param name="request">Request information (EndPoint, internal id, additional data)</param>
        void OnConnectionRequest(ConnectionRequest request);

        /// <summary>
        /// Вызывается при создании комнаты игроком
        /// </summary>
        /// <param name="room">Возвращает ссылку на созданную комнату</param>
        /// <param name="player">Возвращает ссылку на игрока,создавшего комнату</param>
        void OnUserCreateRoom(Room room, Player player);

        /// <summary>
        /// Вызывается при подключении игрока к комнате
        /// </summary>
        /// <param name="room">Возвращает ссылку на комнату</param>
        /// <param name="player">Возвращает ссылку на игрока,подключающегося к комнате</param>
        void OnUserJoinToRoom(Room room, Player player);

        /// <summary>
        /// Вызывается,когда комната удалена
        /// </summary>
        /// <param name="room">Возвращает ссылку на комнату</param>
        void OnDeleteRoom(Room room);

        /// <summary>
        /// Вызывается,когда игрок покинул комнату
        /// </summary>
        /// <param name="room">Возвращает ссылку на комнату</param>
        /// <param name="player">Возвращает ссылку на игрока</param>
        void OnUserLeftRoom(Room room, Player player);

        /// <summary>
        /// Вызывается,когда игра в комнате началась 
        /// </summary>
        /// <param name="room">Возвращает ссылку на комнату</param>
        void OnStartedGame(Room room);


    }

    public class EventBasedNetListener : INetEventListener
    {
        public delegate void OnUserJoined(Player player);
        public delegate void OnUserLeft(Player peer, DisconnectInfo disconnectInfo);
        public delegate void OnNetworkError(IPEndPoint endPoint, int socketErrorCode);
        public delegate void OnGotMessage(Player peer, NetDataReader reader, DeliveryMethod deliveryMethod);
        public delegate void OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetDataReader reader, UnconnectedMessageType messageType);
        public delegate void OnNetworkLatencyUpdate(Player peer, int latency);
        public delegate void OnUserCreateRoom(Room room, Player player);
        public delegate void OnUserJoinToRoom(Room room, Player player);
        public delegate void OnDeleteRoom(Room room);
        public delegate void OnUserLeftRoom(Room room, Player player);
        public delegate void OnStartedGame(Room room);


        public delegate void OnConnectionRequest(ConnectionRequest request);

        public event OnUserJoined PeerConnectedEvent;
        public event OnUserLeft PeerDisconnectedEvent;
        public event OnNetworkError NetworkErrorEvent;
        public event OnGotMessage NetworkReceiveEvent;
        public event OnNetworkReceiveUnconnected NetworkReceiveUnconnectedEvent;
        public event OnNetworkLatencyUpdate NetworkLatencyUpdateEvent;
        public event OnConnectionRequest ConnectionRequestEvent;
        public event OnUserCreateRoom OnUserCreateRoomEvent;
        public event OnUserJoinToRoom OnUserJoinToRoomEvent;
        public event OnDeleteRoom OnDeleteRoomEvent;
        public event OnUserLeftRoom OnUserLeftRoomEvent;
        public event OnStartedGame OnStartedGameEvent;


        void INetEventListener.OnUserJoined(Player peer)
        {
            if (PeerConnectedEvent != null)
                PeerConnectedEvent(peer);
        }

        void INetEventListener.OnUserLeft(Player peer, DisconnectInfo disconnectInfo)
        {
            if (PeerDisconnectedEvent != null)
                PeerDisconnectedEvent(peer, disconnectInfo);
        }

        void INetEventListener.OnNetworkError(IPEndPoint endPoint, int socketErrorCode)
        {
            if (NetworkErrorEvent != null)
                NetworkErrorEvent(endPoint, socketErrorCode);
        }

        void INetEventListener.OnGotMessage(Player peer, NetDataReader reader, DeliveryMethod deliveryMethod)
        {
            if (NetworkReceiveEvent != null)
                NetworkReceiveEvent(peer, reader, deliveryMethod);
        }

        void INetEventListener.OnNetworkReceiveUnconnected(IPEndPoint remoteEndPoint, NetDataReader reader, UnconnectedMessageType messageType)
        {
            if (NetworkReceiveUnconnectedEvent != null)
                NetworkReceiveUnconnectedEvent(remoteEndPoint, reader, messageType);
        }

        void INetEventListener.OnNetworkLatencyUpdate(Player peer, int latency)
        {
            if (NetworkLatencyUpdateEvent != null)
                NetworkLatencyUpdateEvent(peer, latency);
        }

        void INetEventListener.OnConnectionRequest(ConnectionRequest request)
        {
            if (ConnectionRequestEvent != null)
                ConnectionRequestEvent(request);
        }

        void INetEventListener.OnUserCreateRoom(Room room, Player player)
        {
            if (OnUserCreateRoomEvent != null)
                OnUserCreateRoomEvent(room, player);
        }

        void INetEventListener.OnUserJoinToRoom(Room room, Player player)
        {
            if (OnUserJoinToRoomEvent != null)
                OnUserJoinToRoomEvent(room, player);
        }

        void INetEventListener.OnDeleteRoom(Room room)
        {
            if (OnDeleteRoomEvent != null)
                OnDeleteRoomEvent(room);
        }

        void INetEventListener.OnUserLeftRoom(Room room, Player player)
        {
            if (OnUserLeftRoomEvent != null)
                OnUserLeftRoomEvent(room, player);
        }

        void INetEventListener.OnStartedGame(Room room)
        {
            if (OnStartedGameEvent != null)
                OnStartedGameEvent(room);
        }
    }
}
