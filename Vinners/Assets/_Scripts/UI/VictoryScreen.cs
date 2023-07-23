using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using UnityEngine.UI;

public class VictoryScreen : View
{
    [SerializeField] private Button leaveButton;

    public override void Initialise()
    {
        base.Initialise();

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
