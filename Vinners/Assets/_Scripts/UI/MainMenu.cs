using FishNet;
using FishNet.Managing;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Discovery;
using System.Net;

public sealed class MainMenu : View
{
    // host and join both have to load the character select lobby scene
    // options to change keybinds
    // quit game
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button guideButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private NetworkDiscovery networkDiscovery;

    public IPEndPoint ipAddress = null;

    private void OnDestroy()
    {
        networkDiscovery.ServerFoundCallback -= ConnectToIP;
    }

    private void OnDisable()
    {
        networkDiscovery.ServerFoundCallback -= ConnectToIP;
    }

    private void OnEnable()
    {
        networkDiscovery.ServerFoundCallback += ConnectToIP;
    }

    private void ConnectToIP(IPEndPoint address)
    {
        ipAddress = address;

        joinButton.onClick.RemoveAllListeners();

        joinButton.onClick.AddListener(() =>
        {
                networkDiscovery.StopAdvertisingServer();

                networkDiscovery.StopSearchingForServers();

                InstanceFinder.ClientManager.StartConnection(address.ToString());
        });
    }

    private void Start()
    {
        if (networkDiscovery == null) networkDiscovery = FindObjectOfType<NetworkDiscovery>();
        networkDiscovery.ServerFoundCallback += ConnectToIP;

        hostButton.onClick.AddListener(() =>
        {
            InstanceFinder.ServerManager.StartConnection();
            InstanceFinder.ClientManager.StartConnection();
        });

        

        guideButton.onClick.AddListener(() => {
            OfflineUIManager.LocalInstance.Show<GuideMenu>();
        });


        optionsButton.onClick.AddListener(() => OfflineUIManager.LocalInstance.Show<RebindControls>());

        quitButton.onClick.AddListener(() => Application.Quit());
    }

    private void Update()
    {
        joinButton.image.color = ipAddress == null ? Color.grey : Color.white;
        joinButton.interactable = ipAddress != null;
    }
}
    
