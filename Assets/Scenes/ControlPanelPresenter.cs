using System;
using System.Net.Sockets;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes
{
    public class ControlPanelPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject connectingToServerWidget;
        [SerializeField] private Button turnLightOnButton;
        [SerializeField] private Button turnLightOffButton;
        [SerializeField] private Button makeExplosionButton;

        private NetworkClient networkClient;

        private void Start()
        {
            networkClient = new NetworkClient();
            
            turnLightOnButton.OnClickAsObservable().Subscribe(_ => networkClient.SendMessage(NetworkMessage.TurnLightOn));
            turnLightOffButton.OnClickAsObservable().Subscribe(_ => networkClient.SendMessage(NetworkMessage.TurnLightOff));
            makeExplosionButton.OnClickAsObservable().Subscribe(_ => networkClient.SendMessage(NetworkMessage.MakeExplosion));
            
            networkClient.IsConnectedToServer.Subscribe(SetupView);
            
            SetupView(false);
            networkClient.ConnectToServerAsync();
        }

        private void SetupView(bool isConnectedToServer)
        {
            connectingToServerWidget.gameObject.SetActive(!isConnectedToServer);
            turnLightOnButton.interactable = turnLightOffButton.interactable =
                makeExplosionButton.interactable = isConnectedToServer;
        }

        private void OnDestroy()
        {
            networkClient.Dispose();
        }
    }
}
