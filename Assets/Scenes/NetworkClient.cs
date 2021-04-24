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
        private static readonly Encoding ConnectionEncoding = Encoding.UTF8;

        private TcpClient client;
        private NetworkStream dataStream;

        public bool IsConnected => client.Connected;

        public NetworkClient()
        {
            client = new TcpClient();
        }

        public void ConnectToServer()
        {
            client = new TcpClient();
            client.Connect(IP, Port);
            dataStream = client.GetStream();
        }

        public void SendMessage(NetworkMessage message)
        {
            byte[] data = ConnectionEncoding.GetBytes(JsonUtility.ToJson(message)); // использовать .NET JSON?
            dataStream.Write(data, 0, data.Length);
        }

        public void Dispose()
        {
            client?.Close();
            client?.Dispose();
            dataStream?.Dispose();
        }
    }
}
