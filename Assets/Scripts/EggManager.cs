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
    public static EggManager instance;
    
    
    [Header("Elements")] 
    [SerializeField] private Egg eggPrefab;

    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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
        eggInstance.transform.SetParent(transform);
        
    }

    public void ReuseEgg()
    {
        if (!IsServer)
        {
            return;
        }

        if (transform.childCount<=0)
        {
            return;
        }

        transform.GetChild(0).GetComponent<Egg>().Reuse();
    }
    
    
}
