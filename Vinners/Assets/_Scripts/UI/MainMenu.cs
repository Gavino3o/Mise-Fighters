using FishNet;
using FishNet.Managing;
using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {

        hostButton.onClick.AddListener(() =>
        {
            InstanceFinder.ServerManager.StartConnection();
            InstanceFinder.ClientManager.StartConnection();
        });

        joinButton.onClick.AddListener(() => InstanceFinder.ClientManager.StartConnection());

        guideButton.onClick.AddListener(() => {
            Debug.Log("Guide Opened");
            OfflineUIManager.LocalInstance.Show<GuideMenu>();
        });


        optionsButton.onClick.AddListener(() => Debug.Log("Options Menu Opened"));

        quitButton.onClick.AddListener(() => Application.Quit());
    }
}
    
