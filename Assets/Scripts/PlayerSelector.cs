using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerSelector : NetworkBehaviour
{

    private bool isHostTurn;
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

        GameManager.onGameStateChanged += GameStateChangedCallback;
        Egg.onHit += SwitchPlayers;
    }

    private void SwitchPlayers()
    {
        isHostTurn = !isHostTurn;
        
        Initialize();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
        GameManager.onGameStateChanged -= GameStateChangedCallback;
        Egg.onHit -= SwitchPlayers;
    }

    private void GameStateChangedCallback(GameManager.State gameState)
    {
        switch (gameState)
        {
            case GameManager.State.Game:
                Initialize();
                break;
        }
    }

    private void Initialize()
    {
        //look for every player in the game
        PlayerStateManager[] playerStateManagers = FindObjectsOfType<PlayerStateManager>();

        for (int i = 0; i < playerStateManagers.Length; i++)
        {
            if (playerStateManagers[i].GetComponent<NetworkObject>().IsOwnedByServer )
            {
                    //this is the host 
                    //if it's the host turn, enable host and disable client

                    if (isHostTurn)
                    {
                        playerStateManagers[i].Enable();
                    }
                    else
                    {
                        playerStateManagers[i].Disable();
                    }
                
            }

            else
            {
                if (isHostTurn)
                {
                    playerStateManagers[i].Disable();
                }
                else
                {
                    playerStateManagers[i].Enable();
                }
            }
        }
        
    }
}
