using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System;

/*
 * Should contain centralised references to all other components;
 */
public class Character : Unit
{
    public Sprite characterSplash;

    [SyncVar] public Player controllingPlayer;

    [SerializeField] private TextMeshPro usernameDisplay;

    public InputCharacter input;
    public CastCharacter caster;
    public MoveCharacter movement;
    public AttackCharacter attacker;
    public Animator animator;
    public AnimatorCharacter characterAnimator;
    public Rigidbody2D rb;

    public event Action HitEnemy;

    public void HitSuccess()
    {
        HitEnemy?.Invoke();
    }

    private void Awake()
    {
        input = GetComponent<InputCharacter>();
        caster = GetComponent<CastCharacter>();
        movement = GetComponent<MoveCharacter>();
        attacker = GetComponent<AttackCharacter>();
        animator = GetComponent<Animator>();
        characterAnimator = GetComponent<AnimatorCharacter>();
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    /*
     * Disables player input and attacks
     */
    public override void OnDeath()
    {
        if (currHealth > 0) return;
        controllingPlayer.CharacterDeath();
        GetComponent<PlayerInput>().actions.Disable();
        attacker.canAttack = false;
        
    }

    /*
     * Reenables player input and revives the character with full health
     */
    public void Revive()
    {
        GetComponent<PlayerInput>().actions.Enable();
        attacker.canAttack = true;
        ServerRevive();
    }

    [ServerRpc]
    private void ServerRevive()
    {
        TakeDamage(baseStats.maxHealth * -1);
    }

}