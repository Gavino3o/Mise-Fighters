using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine.UI;
using TMPro; 

public class Character : Unit
{
    public Image characterSplash;

    [SyncVar] public Player controllingPlayer;

    [SerializeField] private TextMeshPro usernameDisplay;

    public override void OnStartClient()
    {
        base.OnStartClient();
        if(!IsOwner) return;
        usernameDisplay.GetComponent<TMP_Text>().text = Player.LocalInstance.username;

    }
    // TODO: Alterations to other stats and handling of status effects

}
