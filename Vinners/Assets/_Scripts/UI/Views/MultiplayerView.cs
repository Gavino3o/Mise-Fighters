using UnityEngine;
using UnityEngine.UI;
using FishNet;

public sealed class MultiplayerView : View
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;

    public override void Initialise()
    {
        hostButton.onClick.AddListener(() =>
        {
            InstanceFinder.ServerManager.StartConnection();
            InstanceFinder.ClientManager.StartConnection();
        });

        joinButton.onClick.AddListener(() =>
        {
            InstanceFinder.ClientManager.StartConnection();
        });

        base.Initialise();
        
    }

    public override void Show(object args = null)
    {
        if (args is string message)
        {
            Debug.Log(message);
        }
        base.Show(args);
    }
}
