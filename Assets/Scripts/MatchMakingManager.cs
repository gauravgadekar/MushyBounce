using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class MatchMakingManager : MonoBehaviour
{
    private Lobby lobby;

    [Header("Settings")] [SerializeField] private string _joinCode;
    
    
    async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public async void PlayButtonCallback()
    {
        lobby = await QuickJoinLobby() ?? await CreateLobby();
    }

    private async Task<Lobby> QuickJoinLobby()
    {
        try
        {
            Lobby lobby = await Lobbies.Instance.QuickJoinLobbyAsync();
            JoinAllocation allocation = await RelayService.Instance.JoinAllocationAsync(lobby.Data[_joinCode].Value);
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(allocation.RelayServer.IpV4,(ushort)allocation.RelayServer.Port,allocation.AllocationIdBytes, allocation.Key, allocation.ConnectionData, allocation.HostConnectionData);
            NetworkManager.Singleton.StartClient();
            return lobby;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }    
    
    private async Task<Lobby> CreateLobby()
    {
        try
        {
            return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}
