using System;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgentSync : MonoBehaviour, INetworkSingleActivated
{
    private const MessageType Type = MessageType.NavMeshAgentSync;

    [SerializeField] private NavMeshAgent _navMeshAgent;

    byte[] _buffer = new byte[MessagesLength.Get(Type)];
    public event Action<byte[]> NeedSync;

    private bool _owner;

    public Vector3 Destination => _navMeshAgent.destination;

    public void SetDestination(Vector3 destination)
    {
        if (!_owner)
            return;

        if (_navMeshAgent.enabled == false)
            return;

        _navMeshAgent.SetDestination(destination);

        _buffer[3] = (byte) CommandType.SetDestination;
        _buffer.SetVector3(destination, 4);

        NeedSync?.Invoke(_buffer);
    }

    public void ResetPath()
    {
        if (!_owner)
            return;

        if (_navMeshAgent.enabled == false)
            return;

        _navMeshAgent.ResetPath();
        _buffer[3] = (byte)CommandType.ResetPath;

        NeedSync?.Invoke(_buffer);
    }

    public void Initialize(short id, bool owner)
    {
        _buffer[0] = (byte)Type;
        _buffer.SetId(id);
        _owner = owner;
    }

    public void SetMessage(byte[] message)
    {
        if (message[0] != (byte)Type)
            return;

        var commandType = (CommandType)message[3];

        if (commandType == CommandType.SetDestination)
        {
            var target = message.GetVector3(4);
            _navMeshAgent.SetDestination(target);
        }
        else if (commandType == CommandType.ResetPath)
        {
            _navMeshAgent.ResetPath();
        }
        else
        {
            throw new Exception($"Unsupported nav mesh agent command {commandType}");
        }
    }

    public enum CommandType : byte
    {
        None = 0,
        SetDestination = 1,
        ResetPath = 2
    }
}

