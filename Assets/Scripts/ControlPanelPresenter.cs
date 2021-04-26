using System;
using Replication;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class ControlPanelPresenter : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] private ConnectionConfig connectionConfig;
        
        [Header("Components")]
        [SerializeField] private Button connectToServerButton;
        [SerializeField] private Button turnLightOnButton;
        [SerializeField] private Button turnLightOffButton;
        [SerializeField] private Button makeExplosionButton;

        private CustomTcpClient customTcpClient;

        private void Start()
        {
            customTcpClient = new CustomTcpClient(connectionConfig);
            
            connectToServerButton.OnClickAsObservable().Subscribe(_ => ConnectToServerAsync());
            turnLightOnButton.OnClickAsObservable().Subscribe(_ => SendMessageAsync(NetworkMessage.TurnLightOn));
            turnLightOffButton.OnClickAsObservable().Subscribe(_ => SendMessageAsync(NetworkMessage.TurnLightOff));
            makeExplosionButton.OnClickAsObservable().Subscribe(_ => SendMessageAsync(NetworkMessage.MakeExplosion));

            UpdateView();
        }

        private void UpdateView()
        {
            connectToServerButton.gameObject.SetActive(!customTcpClient.IsConnected);
            connectToServerButton.interactable = !customTcpClient.IsConnected;
            turnLightOnButton.interactable = customTcpClient.IsConnected;
            turnLightOffButton.interactable = customTcpClient.IsConnected;
            makeExplosionButton.interactable = customTcpClient.IsConnected;
        }

        private void ConnectToServerAsync()
        {
            connectToServerButton.interactable = false;
            Observable
                .Start(() =>
                {
                    customTcpClient.ConnectToServer();
                })
                .ObserveOnMainThread()
                .Subscribe(
                    _ => { },
                    err =>
                    {
                        UpdateView();
                        ShowError(err);
                        connectToServerButton.interactable = true;
                    },
                    () =>
                    {
                        UpdateView();
                        connectToServerButton.interactable = true;
                    });
        }

        private void SendMessageAsync(NetworkMessage message)
        {
            Observable
                .Start(() => customTcpClient.SendMessage(message))
                .ObserveOnMainThread()
                .Subscribe(
                    _ => {},
                    err =>
                    {
                        UpdateView();
                        ShowError(err);
                    });
        }

        private void ShowError(Exception ex)
        {
            Debug.LogError(ex.Message);
        }

        private void OnDestroy()
        {
            customTcpClient.Dispose();
        }
    }
}
