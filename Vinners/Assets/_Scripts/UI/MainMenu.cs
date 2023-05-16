using FishNet;
using FishNet.Managing;
using UnityEngine;
using UnityEngine.UI;

public sealed class MainMenu : MonoBehaviour
{
    // host and join both have to load the character select lobby scene
    // options to change keybinds
    // quit game
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private Button optionsButton;
    [SerializeField] private Button quitButton;
    // [SerializeField] private NetworkManager manager;

    private void Start()
    {
        // Instantiate(manager);

        hostButton.onClick.AddListener(() =>
        {
            InstanceFinder.ServerManager.StartConnection();
            InstanceFinder.ClientManager.StartConnection();
        });

        joinButton.onClick.AddListener(() => InstanceFinder.ClientManager.StartConnection());

        optionsButton.onClick.AddListener(() => Debug.Log("Options Menu Opened"));

        quitButton.onClick.AddListener(() => Application.Quit());
    }
}
    
