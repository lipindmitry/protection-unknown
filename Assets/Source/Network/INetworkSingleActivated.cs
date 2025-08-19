using System;

public interface INetworkSingleActivated
{
    event Action<byte[]> NeedSync;
    void Initialize(short id, bool owner);
    void SetMessage(byte[] message);
}

