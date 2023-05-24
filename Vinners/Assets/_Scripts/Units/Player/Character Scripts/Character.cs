using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine.UI;
using TMPro;

/*
 * Should contain centralised references to all other components;
 */
public class Character : Unit
{
    [SerializeField] Image characterSplash;

    [SyncVar] public Player controllingPlayer;

    //TODO: username display is still buggy
    [SerializeField] private TextMeshPro usernameDisplay;
    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        if (!base.Owner.IsLocalClient) return;
        usernameDisplay.text = Player.LocalInstance.username;
    }

}