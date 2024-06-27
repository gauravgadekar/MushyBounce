using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Net;
using System.Linq;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class UIManager : MonoBehaviour
{
    [Header("Panels")] 
    [SerializeField] private GameObject connectionpanel;
    [SerializeField] private GameObject waitingpanel;
    [SerializeField] private GameObject gamepanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

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
            
            case GameManager.State.Win:
                ShowWinpanel();
                break;
            
            case GameManager.State.Lose:
                ShowLosepanel();
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
        
        winPanel.SetActive(false);
        losePanel.SetActive(false);
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
    
    private void ShowWinpanel()
    {
        winPanel.SetActive(true);
    }
    
    private void ShowLosepanel()
    {
        losePanel.SetActive(true);
    }
    
    

    public void HostButtonCallback()
    {
        NetworkManager.Singleton.StartHost();
        ShowWaitingpanel();
    }   
    
    public void ClientButtonCallback()
    {
        //grab ip address of player
        string ipAddress = IPManager.instance.GetInputIp();
        
        //congif the network manager
        UnityTransport utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.SetConnectionData(ipAddress, 7777);
        
        NetworkManager.Singleton.StartClient();
        ShowWaitingpanel();
    }

    public void NextButtonCallback()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        NetworkManager.Singleton.Shutdown();
    }
}
