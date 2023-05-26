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
    public Image characterSplash;

    [SyncVar] public Player controllingPlayer;

    //TODO: username display is still buggy
    [SerializeField] private TextMeshPro usernameDisplay;

    public InputCharacter input;
    public CastCharacter caster;
    public MoveCharacter movement;
    public AttackCharacter attacker;
    public Rigidbody2D rb;

    private void Awake()
    {
        input = GetComponent<InputCharacter>();
        caster = GetComponent<CastCharacter>();
        movement = GetComponent<MoveCharacter>();
        attacker = GetComponent<AttackCharacter>();
        rb = GetComponent<Rigidbody2D>();
    }
    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        if (!base.Owner.IsLocalClient) return;
        usernameDisplay.text = Player.LocalInstance.username;
    }

    public override void Die()
    {
        controllingPlayer.CharacterDeath();
    }

    public void Revive()
    {
        currHealth = baseStats.maxHealth;
    }

}