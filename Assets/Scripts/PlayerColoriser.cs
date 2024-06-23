using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerColoriser : NetworkBehaviour
{

    [Header("Elements")] 
    [SerializeField] private SpriteRenderer[] renderers;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        if (!IsServer && IsOwner)
        {
            ColoriseServerRpc(Color.red);
        }
    }

    [ServerRpc]
    private void ColoriseServerRpc(Color color)
    {
        ColoriseClientRpc(color);
    }
    
    [ClientRpc]
    private void ColoriseClientRpc(Color color)
    {
        foreach (SpriteRenderer renderer in renderers)
        {
            renderer.color = color;
        }
    }

}
