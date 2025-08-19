using System;
using UnityEngine;

public class HealthSync : MonoBehaviour, INetworkSingleActivated
{
    [SerializeField] private Unit _unit;

    public event Action<byte[]> NeedSync;

    private const MessageType Type = MessageType.NavMeshAgentSync;
    byte[] _buffer = new byte[MessagesLength.Get(Type)];
    private bool _owner;

    public void Initialize(short id, bool owner)
    {
        _owner = owner;
        if (_owner)
        {
            _unit.Health.Changed += OnHealthChanged;
            _buffer[0] = (byte)Type;
            _buffer.SetId(id);
        }
    }

    private void OnHealthChanged(Parametr parametr)
    {
        _buffer.SetFloat(parametr.Current, 3);
        NeedSync?.Invoke(_buffer);
    }

    public void SetMessage(byte[] message)
    {
        if (message[0] != (byte)Type)
            return;

        float value = BitConverter.ToSingle(message, 3);
        _unit.Health.Set(value);
    }
}

