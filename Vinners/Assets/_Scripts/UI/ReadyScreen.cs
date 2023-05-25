using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReadyScreen : View
{
    public Button readyButton;

    public override void Initialise()
    {
        base.Initialise();

        readyButton.onClick.AddListener(() => Player.LocalInstance.ServerSetLockIn(!Player.LocalInstance.isLockedIn));
    }
}
