using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _prefabs;
    [SerializeField] private TcpClient _tcpClient;

    public event Action<GameObject, short> OwnedSpawned;
    public event Action<GameObject, short> ForienSpawned;

    private short _id;
    private readonly IList<short> _spawned = new List<short>();

    public bool Main {  get; private set; }

    private void Awake()
    {
        _tcpClient.MessageReceived += OnTcpMessageReceived;
        _id = 1;
    }

    private void OnTcpMessageReceived(byte[] message)
    {
        if (message[0] == (byte)MessageType.ObjectSpawned)
        {
            short id = BitConverter.ToInt16(message, 1);
            short inctance = BitConverter.ToInt16(message, 3);
            var position = message.GetVector3(5);
            var rotation = message.GetQuaternion(17);

            SpawnForienObject(id, inctance, position, rotation);
        }
        else if (message[0] == (byte)MessageType.Number)
        {
            Main = message[1] == 1;
            _id += (short)(message[1] * 1000);
        }
    }

    public GameObject SpawnOwnedObject(short instanceId, Vector3 position, Quaternion rotation)
    {
        var prefab = _prefabs.FirstOrDefault(x => x.GetComponent<Unique>().Id == instanceId);
        var newSpawned = Instantiate(prefab, position, rotation);

        OwnedSpawned?.Invoke(newSpawned, _id);
       
        byte[] message = GetSpawnMessage(_id, instanceId, position, rotation);
        _tcpClient.SendMessageToServer(message);

        _spawned.Add(_id);
        _id++;

        return newSpawned;
    }
    private void SpawnForienObject(short id, short instanceId, Vector3 position, Quaternion rotation)
    {
        if (_spawned.Contains(id))
            return;

        _spawned.Add(id);

        var prefab = _prefabs.FirstOrDefault(x => x.GetComponent<Unique>().Id == instanceId);
        var newSpawned = Instantiate(prefab, position, rotation);

        var mover = newSpawned.GetComponent<Commander>();
        if (mover != null)
            Destroy(mover);

        ForienSpawned?.Invoke(newSpawned, id);

    }
    private byte[] GetSpawnMessage(short id, short instanceId, Vector3 position, Quaternion rotation)
    {
        byte[] _buffer = new byte[33];
        _buffer[0] = (byte)MessageType.ObjectSpawned;
        
        byte[] idBytes = BitConverter.GetBytes(id);
        Buffer.BlockCopy(idBytes, 0, _buffer, 1, 2);

        byte[] instanceBytes = BitConverter.GetBytes(instanceId);
        Buffer.BlockCopy(instanceBytes, 0, _buffer, 3, 2);

        _buffer.SetVector3(position, 5);
        _buffer.SetQuartenion(rotation, 17);

        return _buffer;
    }
}

