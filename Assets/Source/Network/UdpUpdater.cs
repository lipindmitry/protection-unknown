using System;
using System.Collections.Generic;
using UnityEngine;

public class UdpUpdater : MonoBehaviour
{
    [SerializeField] private NetworkSpawner _spawner;
    [SerializeField] private UdpClient _udpClient;
    
    [SerializeField] private float _syncCountInSec = 0.1f;

    private readonly Dictionary<short, IEnumerable<INetworkSynchronizable>> _ownedObjects = new();
    private readonly Dictionary<short, IEnumerable<INetworkSynchronizable>> _forienObjects = new();
    
    private float _counter;

    private void Start()
    {
        _spawner.OwnedSpawned += OnOwnedSpawned;
        _spawner.ForienSpawned += OnForienSpawned;
        _udpClient.MessageRecieved += OnMessageUdpReceived;
    }

    private void OnMessageUdpReceived(byte[] message)
    {
        short id = BitConverter.ToInt16(message, 1);
        if (_forienObjects.ContainsKey(id))
            foreach (var networkSynchronizable in _forienObjects[id])
                networkSynchronizable.SetMessage(message);
    }

    private void OnForienSpawned(GameObject gameObject, short id)
    {
        var networkSynchronizables = gameObject.GetComponentsInChildren<INetworkSynchronizable>();
        foreach (var network in networkSynchronizables)
            network.Initialize(id, false);
        _forienObjects[id] = networkSynchronizables;
    }

    private void OnOwnedSpawned(GameObject gameObject, short id)
    {
        var networkSynchronizables = gameObject.GetComponentsInChildren<INetworkSynchronizable>();
        foreach (var network in networkSynchronizables)
            network.Initialize(id, true);
        _ownedObjects[id] = networkSynchronizables;
    }

    private void Update()
    {
        _counter += _syncCountInSec * Time.deltaTime;

        if (_counter > 1)
        {
            foreach (var networkSynchronizables in _ownedObjects.Values)
            {
                foreach (var networkSynchronizable in networkSynchronizables)
                {
                    var message = networkSynchronizable.GetMessage();
                    _udpClient.SendMessageToServer(message);
                }
            }
            _counter = 0;
        }
    }
}

