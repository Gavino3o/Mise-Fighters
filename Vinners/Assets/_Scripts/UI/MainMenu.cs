using FishNet;
using FishNet.Managing.Scened;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // host and join both have to load the character select lobby scene
    // options to change keybinds
    // quit game
    [SerializeField] private const string nextScene = "CharacterSelect";

    public void StartHost()
    {
        InstanceFinder.ServerManager.StartConnection();
        InstanceFinder.ClientManager.StartConnection();
    }

    public void StartClient()
    {
        InstanceFinder.ClientManager.StartConnection();
    }
}
