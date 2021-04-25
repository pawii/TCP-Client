using System;
using System.Net.Sockets;
using System.Text;
using Replication;
using UnityEngine;

namespace Scripts
{
    public class NetworkClient : IDisposable
    {
        private readonly ConnectionConfig connectionConfig;
        private TcpClient client;
        private NetworkStream dataStream;

        public bool IsConnected => client.Connected;

        public NetworkClient(ConnectionConfig connectionConfig)
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
            byte[] data = connectionConfig.Encoding.GetBytes(JsonUtility.ToJson(message)); // использовать .NET JSON?
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
