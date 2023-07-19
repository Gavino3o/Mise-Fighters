using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System;
using FishNet.Connection;

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
        GameManager.Instance.DecrementLives();
        isInvicible = true;
        controllingPlayer.CharacterDeath();
        GetComponent<PlayerInput>().DeactivateInput();
        attacker.canAttack = false;
     
    }

    /*
     * Reenables player input and revives the character with half health
     */
    public void Revive()
    {
        
        ServerRevive();
        GetComponent<PlayerInput>().ActivateInput();
        attacker.canAttack = true;
    }

    [ServerRpc]
    private void ServerRevive()
    {
        isInvicible = false;
        TakeDamage(baseStats.maxHealth * -0.75f);
    }
    
}