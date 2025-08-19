using System;
using UnityEngine;

public class TransformSync : MonoBehaviour, INetworkSynchronizable
{
    [SerializeField] private Transform _transform;

    byte[] _buffer = new byte[MessagesLength.Get(MessageType.TransformSync)];
    private bool _owner;

    public bool Owner => _owner;

    void SerializeDataManual()
    {
        _buffer.SetVector3(_transform.position, 3);
        _buffer.SetQuartenion(_transform.rotation, 15);
    }

    public void Initialize(short id, bool owner)
    {
        _buffer[0] = (byte)MessageType.TransformSync;
        _owner = owner;
        byte[] idBytes = BitConverter.GetBytes(id);
        Buffer.BlockCopy(idBytes, 0, _buffer, 1, 2);
    }

    public byte[] GetMessage()
    {
        SerializeDataManual();
        return _buffer;
    }

    public void SetMessage(byte[] message)
    {
        if ((MessageType)message[0] != MessageType.TransformSync)
            return;

        _transform.position = message.GetVector3(3);
        _transform.rotation = message.GetQuaternion(15);
    }
}

