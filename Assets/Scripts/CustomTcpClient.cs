using System;
using System.Net.Sockets;
using Replication;
using UnityEngine;

namespace Scripts
{
    public class CustomTcpClient : IDisposable
    {
        private readonly ConnectionConfig connectionConfig;
        private TcpClient client;
        private NetworkStream dataStream;

        public bool IsConnected => client.Connected;

        public CustomTcpClient(ConnectionConfig connectionConfig)
        {
            this.connectionConfig = connectionConfig;
            client = new TcpClient();
        }

        public void ConnectToServer()
        {
            client = new TcpClient();
            client.Connect(connectionConfig.IP, connectionConfig.Port);
            dataStream = client.GetStream();
        }

        public void SendMessage(NetworkMessage message)
        {
            var data = connectionConfig.Encoding.GetBytes(JsonUtility.ToJson(message));
            dataStream.Write(data, 0, data.Length);
        }

        public void Dispose()
        {
            dataStream?.Close();
            dataStream?.Dispose();
            client?.Close();
            client?.Dispose();
        }
    }
}
