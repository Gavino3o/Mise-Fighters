using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet;

public class PauseMenu : View
{

    public Button readyButton;
    public Button leaveButton;
    public override void Initialise()
    {
        base.Initialise();

        readyButton.onClick.AddListener(() => UIManager.LocalInstance.Show<GameInfo>());

        if (InstanceFinder.IsHost)
        {
            leaveButton.onClick.AddListener(() =>
            {
                InstanceFinder.ServerManager.StopConnection(false);
                InstanceFinder.ClientManager.StopConnection();

            });

        }
        else
        {

            leaveButton.onClick.AddListener(() => {
                InstanceFinder.ClientManager.StopConnection();
            });

        }
    }
}
