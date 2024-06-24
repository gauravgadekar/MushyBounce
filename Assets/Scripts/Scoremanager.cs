using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class Scoremanager : NetworkBehaviour
{
    
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
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        NetworkManager.OnServerStarted -= NetworkManager_OnServerStarted;
        Egg.onFellInWater -= EggFellInWaterCallback;
    }

    private void EggFellInWaterCallback()
    {
        
    }



}
