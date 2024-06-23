using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Panels")] 
    [SerializeField] private GameObject connectionpanel;
    [SerializeField] private GameObject waitingpanel;
    [SerializeField] private GameObject gamepanel;

    private void Start()
    {
        ShowConnectionpanel();

        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameManager.State gameState)
    {
        switch (gameState)
        {
            case GameManager.State.Game:
                ShowGamepanel();
                break;
        }
    }

    private void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    private void ShowConnectionpanel()
    {
        connectionpanel.SetActive(true);
        waitingpanel.SetActive(false);
        gamepanel.SetActive(false);
    }
    
    private void ShowWaitingpanel()
    {
        connectionpanel.SetActive(false);
        waitingpanel.SetActive(true);
        gamepanel.SetActive(false);
    }
    
    private void ShowGamepanel()
    {
        connectionpanel.SetActive(false);
        waitingpanel.SetActive(false);
        gamepanel.SetActive(true);
    }

    public void HostButtonCallback()
    {
        NetworkManager.Singleton.StartHost();
        ShowWaitingpanel();
    }   
    
    public void ClientButtonCallback()
    {
        NetworkManager.Singleton.StartClient();
        ShowWaitingpanel();
    }
}
