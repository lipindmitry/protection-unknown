using System;
using System.Collections.Generic;
using UnityEngine;

public class TcpUpdater : MonoBehaviour
{
    [SerializeField] private NetworkSpawner _spawner;
    [SerializeField] private TcpClient _tcpClient;

    private readonly Dictionary<short, IEnumerable<INetworkSingleActivated>> _ownedObjects = new();
    private readonly Dictionary<short, IEnumerable<INetworkSingleActivated>> _forienObjects = new();

    private void Start()
    {
        _spawner.OwnedSpawned += OnOwnedSpawned;
        _spawner.ForienSpawned += OnForienSpawned;
        _tcpClient.MessageReceived += OnMessageUdpReceived;
    }

    private void OnMessageUdpReceived(byte[] message)
    {
        var type = (MessageType)message[0];

        if (type == MessageType.ObjectSpawned || type == MessageType.Number)
            return;

        short id = BitConverter.ToInt16(message, 1);
        if (_forienObjects.ContainsKey(id))
            foreach (var networkSynchronizable in _forienObjects[id])
                networkSynchronizable.SetMessage(message);
    }

    private void OnForienSpawned(GameObject gameObject, short id)
    {
        var networkSingleActivateds = gameObject.GetComponentsInChildren<INetworkSingleActivated>();
        foreach (var network in networkSingleActivateds)
            network.Initialize(id, false);
        _forienObjects[id] = networkSingleActivateds;
    }

    private void OnOwnedSpawned(GameObject gameObject, short id)
    {
        var networkSingleActivateds = gameObject.GetComponentsInChildren<INetworkSingleActivated>();
        foreach (var network in networkSingleActivateds)
        {
            network.Initialize(id, true);
            network.NeedSync += OnNeedSync;
        }
        _ownedObjects[id] = networkSingleActivateds;
    }

    private void OnNeedSync(byte[] message)
    {
        _tcpClient.SendMessageToServer(message);
    }
}

