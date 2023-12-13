using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet;
using TMPro;

public class ReadyScreen : View
{
    public Button readyButton;
    public Button startGameButton;

    public override void Initialise()
    {
        base.Initialise();

        readyButton.onClick.AddListener(() => Player.LocalInstance.ServerSetLockIn(!Player.LocalInstance.isLockedIn));

        /*
         * Only the Host should have access to the start button
         */
        if (InstanceFinder.IsHost)
        {
            if (startGameButton != null) startGameButton.gameObject.SetActive(true);

            startGameButton.onClick.AddListener(() => {
                GameManager.Instance.LoadNextScene();
            });

        }
        else
        {
            if (startGameButton != null) startGameButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (!Initialised) return;

        readyButton.image.color = Player.LocalInstance.isLockedIn ? Color.grey : Color.white;

        // Should only be able to start game if all players in the lobby are ready.
        startGameButton.interactable = GameManager.Instance.canStart;
    }
}
