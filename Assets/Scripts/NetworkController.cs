using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class NetworkController : NetworkBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private GameObject connectionPanel;
    private void Awake()
    {
        hostButton.onClick.AddListener(Host);
        joinButton.onClick.AddListener(Join);
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    void Host()
    {
        NetworkManager.Singleton.StartHost();
        connectionPanel.SetActive(false);
    }

    void Join()
    {
        NetworkManager.Singleton.StartClient();
        connectionPanel.SetActive(false);
    }


}
