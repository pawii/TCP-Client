using System;
using System.Threading;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Scenes
{
    public class ControlPanelPresenter : MonoBehaviour
    {
        [SerializeField] private Button connectToServerButton;
        [SerializeField] private Button turnLightOnButton;
        [SerializeField] private Button turnLightOffButton;
        [SerializeField] private Button makeExplosionButton;

        private NetworkClient networkClient;

        private void Start()
        {
            networkClient = new NetworkClient();
            
            connectToServerButton.OnClickAsObservable().Subscribe(_ => ConnectToServerAsync());
            turnLightOnButton.OnClickAsObservable().Subscribe(_ => SendMessageAsync(NetworkMessage.TurnLightOn));
            turnLightOffButton.OnClickAsObservable().Subscribe(_ => SendMessageAsync(NetworkMessage.TurnLightOff));
            makeExplosionButton.OnClickAsObservable().Subscribe(_ => SendMessageAsync(NetworkMessage.MakeExplosion));

            UpdateView();
        }

        private void UpdateView()
        {
            connectToServerButton.gameObject.SetActive(!networkClient.IsConnected);
            connectToServerButton.interactable = !networkClient.IsConnected;
            turnLightOnButton.interactable = networkClient.IsConnected;
            turnLightOffButton.interactable = networkClient.IsConnected;
            makeExplosionButton.interactable = networkClient.IsConnected;
        }

        private void ConnectToServerAsync()
        {
            connectToServerButton.interactable = false;
            Observable
                .Start(() =>
                {
                    networkClient.ConnectToServer();
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
                .Start(() => networkClient.SendMessage(message))
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
            networkClient.Dispose();
        }
    }
}
