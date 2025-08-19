using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class UdpClient : MonoBehaviour
{
    public event Action<byte[]> MessageRecieved;

    [SerializeField] private string _serverIp = "127.0.0.1";
    [SerializeField] private int _serverPort = 3000;
    [SerializeField] private int _localPort = 3001; // Локальный порт для получения сообщений

    private System.Net.Sockets.UdpClient _udpClient;
    private IPEndPoint _serverEndPoint;
    private CancellationTokenSource _cts;
    private bool _isConnected;
    private List<byte[]> _messagesToSend = new();

    private void Start()
    {
        ConnectToServer();
    }

    private void OnDestroy()
    {
        Disconnect();
    }

    private async void LateUpdate()
    {
        if (!_isConnected) 
            return;

        try
        {
            var message = GetMessage();
            if (message.Length != 0)
            {
                await _udpClient.SendAsync(message, message.Length, _serverEndPoint);
                //if ((MessageType)message[0] == MessageType.TransformSync)
                //    Debug.Log($"Отправлен вектор: {message.GetVector3(3)}; поворот: {message.GetQuaternion(15)}");
            }
            _messagesToSend.Clear();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка отправки UDP сообщения: {ex.Message}");
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

    private void ConnectToServer()
    {
        try
        {
            _serverEndPoint = new IPEndPoint(IPAddress.Parse(_serverIp), _serverPort);
            _udpClient = new System.Net.Sockets.UdpClient(_localPort + new System.Random().Next(0, 900));
            _cts = new CancellationTokenSource();
            _isConnected = true;

            Debug.Log($"UDP клиент запущен на порту {_udpClient.Client.LocalEndPoint}, подключение к серверу {_serverIp}:{_serverPort}");

            // Запускаем задачу для чтения сообщений
            _ = ReceiveMessagesAsync(_cts.Token);

            SendMessageToServer(new byte[] {(byte)MessageType.Connect});
        }
        catch (Exception ex)
        {
            Debug.LogError($"Ошибка инициализации UDP клиента: {ex.Message}");
            Disconnect();
        }
    }

    private async Task ReceiveMessagesAsync(CancellationToken token)
    {
        while (_isConnected && !token.IsCancellationRequested)
        {
            UdpReceiveResult result;
            try
            {
                // Ожидаем получение сообщения
                result = await _udpClient.ReceiveAsync().WithCancellation(token);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ошибка приёма UDP сообщения: {ex.Message}");
                break;
            }
            int cursor = 0;
            while (result.Buffer[cursor] != 0)
            {
                var type = (MessageType)result.Buffer[cursor];
                int length = MessagesLength.Get(type);
                byte[] message = new byte[length];
                Array.Copy(result.Buffer, cursor, message, 0, length);
                MessageRecieved?.Invoke(message);

                cursor += length;

                //if (type == MessageType.TransformSync)
                //    Debug.Log($"Получен вектор: {message.GetVector3(3)}; поворот: {message.GetQuaternion(15)}");
            }

        }
    }

    public void SendMessageToServer(byte[] message)
    {
        _messagesToSend.Add(message);
    }

    private async void Disconnect()
    {
        _isConnected = false;
        var message = new byte[] { (byte)MessageType.Disconnect };
        await _udpClient.SendAsync(message, message.Length, _serverEndPoint);
        _cts?.Cancel();
        _udpClient?.Close();

        Debug.Log("UDP клиент отключен");
    }
}

// Расширение для добавления поддержки CancellationToken в ReceiveAsync
public static class UdpClientExtensions
{
    public static async Task<UdpReceiveResult> ReceiveAsync(this System.Net.Sockets.UdpClient client,
        CancellationToken cancellationToken)
    {
        var task = client.ReceiveAsync();
        var tcs = new TaskCompletionSource<bool>();

        using (cancellationToken.Register(() => tcs.TrySetCanceled()))
        {
            var completedTask = await Task.WhenAny(task, tcs.Task);
            if (completedTask == tcs.Task)
            {
                throw new OperationCanceledException(cancellationToken);
            }
            return await task;
        }
    }

    public static Task<UdpReceiveResult> WithCancellation(this Task<UdpReceiveResult> task, CancellationToken cancellationToken)
    {
        return task.IsCompleted ? task : Task.Run(() =>
        {
            var tcs = new TaskCompletionSource<UdpReceiveResult>();
            cancellationToken.Register(() => tcs.TrySetCanceled());
            return Task.WhenAny(task, tcs.Task).Unwrap();
        });
    }
}