namespace LiteNetLib.Utils
{
    public interface INetSerializable
    {
        void Serialize(Message writer);
        void Deserialize(NetDataReader reader);
    }
}
