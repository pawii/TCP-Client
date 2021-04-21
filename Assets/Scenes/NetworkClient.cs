using System;
using System.Net.Sockets;
using System.Text;
using UniRx;
using UnityEngine;

namespace Scenes
{
    public class NetworkClient : IDisposable
    {
        private const int Port = 8888;
        private const string IP = "127.0.0.1";

        public ReadOnlyReactiveProperty<bool> IsConnectedToServer => isConnectedToServer.ToReadOnlyReactiveProperty();

        private readonly BoolReactiveProperty isConnectedToServer;
        private TcpClient client;
        private NetworkStream dataStream;

        public NetworkClient()
        {
            isConnectedToServer = new BoolReactiveProperty(false);
        }

        public async void ConnectToServerAsync()
        {
            client = new TcpClient();
            await client.ConnectAsync(IP, Port);
            isConnectedToServer.Value = true;
            dataStream = client.GetStream();
        }

        public void SendMessage(NetworkMessage message)
        {
            byte[] data = Encoding.UTF8.GetBytes(JsonUtility.ToJson(message)); // использовать .NET JSON?
            dataStream.Write(data, 0, data.Length);
        }

        public void Dispose()
        {
            client?.Close();
            client?.Dispose();
            dataStream?.Dispose();
            IsConnectedToServer?.Dispose();
        }
    }
}
