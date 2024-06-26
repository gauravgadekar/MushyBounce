using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Scoremanager : NetworkBehaviour
{

    [Header("Elements")] 
    private int hostScore;
    private int clientScore;

    [SerializeField] private TextMeshProUGUI scoreText;
    
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        NetworkManager.OnServerStarted += NetworkManager_OnServerStarted;
    }

    private void NetworkManager_OnServerStarted()
    {
        if (!IsServer)
        {
            return;
        }

        Egg.onFellInWater += EggFellInWaterCallback;
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    private void GameStateChangedCallback(GameManager.State gamestate)
    {

        switch (gamestate)
        {
            case GameManager.State.Game:
                ResetScore();
                break;
        }
        
    }

    private void ResetScore()
    {
        hostScore = 0;
        clientScore = 0;
        
        UpdateScoreClientRpc(hostScore,clientScore);
    }
    
    

    public override void OnDestroy()
    {
        base.OnDestroy();
        NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
        Egg.onFellInWater -= EggFellInWaterCallback;
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }

    private void EggFellInWaterCallback()
    {
        if (PlayerSelector.instance.isHostturn())
        {
            clientScore++;
        }
        else
        {
            hostScore++;
        }
        
        UpdateScoreClientRpc(hostScore,clientScore);
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        UpdateScoreTextClientRpc();
    }

    private void Start()
    {
        UpdateScoreText();
    }
    
    [ClientRpc]
    private void UpdateScoreClientRpc(int hostScore, int clientScore )
    {
        this.hostScore = hostScore;
        this.clientScore = clientScore;

    }

    [ClientRpc]
    private void UpdateScoreTextClientRpc()
    {
        scoreText.text= "<color=#0055ff>"+hostScore+"</color>-<color=#E52020>" + clientScore + "</color>";
    }
    
}
