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
        }

        public void SendMessage(NetworkMessage message)
        {
            using (var dataStream = client.GetStream())
            {
                byte[] data = connectionConfig.Encoding.GetBytes(JsonUtility.ToJson(message)); // использовать .NET JSON?
                dataStream.Write(data, 0, data.Length);
            }
        }

        public void Dispose()
        {
            client?.Close();
            client?.Dispose();
        }
    }
}
