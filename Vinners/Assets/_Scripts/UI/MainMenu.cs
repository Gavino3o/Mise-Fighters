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
    [SerializeField] public NetworkDiscovery networkDiscovery;

    public IPEndPoint ipAddress;
    private void StoreAddress(IPEndPoint address)
    {
        ipAddress = address;

    }

    private void Start()
    {
        if (networkDiscovery == null) networkDiscovery = FindObjectOfType<NetworkDiscovery>();
        networkDiscovery.ServerFoundCallback += StoreAddress;

        hostButton.onClick.AddListener(() =>
        {
            InstanceFinder.ServerManager.StartConnection();
            InstanceFinder.ClientManager.StartConnection();
        });


        joinButton.onClick.AddListener(() =>
        {
            ConnectToStoredAddress();
        });



        guideButton.onClick.AddListener(() => {
            OfflineUIManager.LocalInstance.Show<GuideMenu>();
        });


        optionsButton.onClick.AddListener(() => OfflineUIManager.LocalInstance.Show<RebindControls>());

        quitButton.onClick.AddListener(() => Application.Quit());
    }

    private void ConnectToStoredAddress()
    {
        networkDiscovery.StopAdvertisingServer();

        networkDiscovery.StopSearchingForServers();

        InstanceFinder.ClientManager.StartConnection(ipAddress.ToString());
    }

    private void Update()
    {
        joinButton.image.color = ipAddress != null ? Color.white : Color.grey;
        joinButton.interactable = ipAddress != null;
    }
}
    
