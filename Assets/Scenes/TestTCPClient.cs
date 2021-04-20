using System;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace Scenes
{
    public class TestTCPClient : MonoBehaviour
    {
        private const int Port = 8888;
        private const string IP = "127.0.0.1";

        private TcpClient client;
        private NetworkStream stream;
        
        private void Start()
        {
            client = new TcpClient();
            client.Connect(IP, Port);
            stream = client.GetStream();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.A))
            {
                OnTurnLightOnClick();
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                OnTurnLightOffClick();
            }
            if (Input.GetKeyUp(KeyCode.D))
            {
                OnExplosionClick();
            }
        }

        public void OnTurnLightOnClick() => SendMessage("Turn Light On");
        public void OnTurnLightOffClick() => SendMessage("Turn Light Off");
        public void OnExplosionClick() => SendMessage("Explosion");

        private void SendMessage(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        private void OnDestroy()
        {
            stream.Close();
            client.Close();
        }
    }
}
