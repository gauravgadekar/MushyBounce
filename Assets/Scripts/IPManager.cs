using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Net;
using System.Linq;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;


public class IPManager : MonoBehaviour
{
    [Header("Elements")] 
    [SerializeField] private TextMeshProUGUI ipText;
    [SerializeField] private TMP_InputField ipInputField;
    public static IPManager instance;
    
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
        ipText.text = GetLocalIPv4();

        UnityTransport utp = NetworkManager.Singleton.GetComponent<UnityTransport>();
        utp.SetConnectionData(GetLocalIPv4(), 7777);
    }

    public string GetInputIp()
    {
        return ipInputField.text;
    }
    
    public string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName())
            .AddressList.First(
                f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            .ToString();
    }
    
    
}
