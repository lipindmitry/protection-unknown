using UnityEngine;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class TcpClient : MonoBehaviour
{
    public event Action<byte[]> MessageReceived;

    [SerializeField] private string _serverIp = "127.0.0.1";
    [SerializeField] private int _serverPort = 4000;

    private System.Net.Sockets.TcpClient _client;
    private NetworkStream _stream;
    private CancellationTokenSource _cts;
    private bool _isConnected;
    private readonly List<byte[]> _messagesToSend = new();

    private void Start()
    {
        ConnectToServer();
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    public async void LateUpdate()
    {
        if (!_isConnected) return;

        try
        {
            var message = GetMessage();
            if (message.Length != 0)
            {
                await _stream.WriteAsync(message, 0, message.Length, _cts.Token);
                //Debug.Log($"Отправлено сообщение: {(MessageType)message[0]}");
                //if ((MessageType)message[0] == MessageType.NavMeshAgentSync)
                //    Debug.Log($"Отправлено: Таргет позиция: {message.GetVector3(4)}");
            }
            _messagesToSend.Clear();

        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка отправки: {ex.Message}");
        }
    }

    private byte[] GetMessage()
    {
        int totalLength = _messagesToSend.Sum(x => x.Length);
        byte[] result = new byte[totalLength];
        int offset = 0;

        foreach (byte[] array in _messagesToSend)
        {
            Buffer.BlockCopy(array, 0, result, offset, array.Length);
            offset += array.Length;
        }

        return result;
    }

    public async void ConnectToServer()
    {
        try
        {
            _client = new System.Net.Sockets.TcpClient();
            _cts = new CancellationTokenSource();

            Debug.Log($"Подключаемся к серверу {_serverIp}:{_serverPort}...");
            await _client.ConnectAsync(_serverIp, _serverPort);

            _stream = _client.GetStream();
            _isConnected = true;

            Debug.Log("Подключено!");

            // Запускаем задачу для чтения сообщений
            _ = ReceiveMessagesAsync(_cts.Token);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка подключения: {ex.Message}");
            Disconnect();
        }
    }

    private async Task ReceiveMessagesAsync(CancellationToken token)
    {
        byte[] buffer = new byte[1024];
        while (_isConnected && !token.IsCancellationRequested)
        {
            try
            {
                int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length, token);
                if (bytesRead == 0) 
                    break; // Сервер закрыл соединение

                int cursor = 0;
                while (buffer[cursor] != 0)
                {
                    var type = (MessageType)buffer[cursor];
                    Debug.Log($"Получено сообщение от сервера типа: {type}, cursor: {cursor}, bytesRead: {bytesRead}");
                    int length = MessagesLength.Get(type);
                    byte[] message = new byte[length];
                    Array.Copy(buffer, cursor, message, 0, length);
                    MessageReceived?.Invoke(message);

                    //if (type == MessageType.NavMeshAgentSync)
                    //    Debug.Log($"Получено: Таргет позиция: {message.GetVector3(4)}");

                    cursor += length;
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (IOException)
            {
                break;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка приёма: {ex}");
                break;
            }
        }
    }

    public void SendMessageToServer(byte[] message)
    {
        _messagesToSend.Add(message);
    }

    private void Disconnect()
    {
        _isConnected = false;
        _cts?.Cancel();
        _stream?.Close();
        _client?.Close();

        Debug.Log("Система: Отключено от сервера");
    }
}