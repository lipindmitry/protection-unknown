public interface INetworkSynchronizable
{
    void Initialize(short id, bool owner);
    byte[] GetMessage();
    void SetMessage(byte[] message);
}

