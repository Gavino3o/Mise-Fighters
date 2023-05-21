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

    [SerializeField] private TextMeshPro usernameDisplay;

    public InputCharacter inputCharacter;
    public AttackCharacter attackCharacter;
    public MoveCharacter moveCharacter;
    public CastCharacter castCharacter;

    public Rigidbody2D rb;
    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        if (!base.Owner.IsLocalClient) return;
        usernameDisplay.text = Player.LocalInstance.username;

        inputCharacter = GetComponent<InputCharacter>();
        attackCharacter = GetComponent<AttackCharacter>();
        moveCharacter = GetComponent<MoveCharacter>();
        castCharacter = GetComponent<CastCharacter>();

        rb = GetComponent<Rigidbody2D>();
        controllingPlayer = Player.LocalInstance;

    }
    // TODO: Alterations to other stats and handling of status effects

}