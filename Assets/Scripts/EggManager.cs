using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.Netcode;
using UnityEngine;
using Unity.Netcode;
using Unity.VisualScripting;

public class EggManager : NetworkBehaviour
{
    [Header("Elements")] 
    [SerializeField] private Egg eggPrefab;

    private void Start()
    {
        GameManager.onGameStateChanged += GameStateChangedCallback;
    }

    public override void OnDestroy()
    {
        GameManager.onGameStateChanged -= GameStateChangedCallback;
    }
    
    private void GameStateChangedCallback(GameManager.State gameState)
    {
        switch (gameState)
        {
            case GameManager.State.Game:
                SpawnEgg();
                break;
        }
    }

    private void SpawnEgg()
    {
        if (!IsServer)
        {
            return;
        }

        Egg eggInstance = Instantiate(eggPrefab, Vector2.up * 5, quaternion.identity);
        
        eggInstance.GetComponent<NetworkObject>().Spawn();
        
    }
    
    
}
