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

        string rebinds = PlayerPrefs.GetString("rebinds", string.Empty);

        if (string.IsNullOrEmpty(rebinds)) return;

        GetComponent<PlayerInput>().actions.LoadBindingOverridesFromJson(rebinds);
    }

    /*
    * Disables player input and attacks
    */
    public override void OnDeath()
    {
        if (currHealth > 0) return;
        GameManager.Instance.DecrementLives();
        
        ShowRespawn(Owner);
        ToggleInput(Owner, false);
        isInvicible = true;
        attacker.canAttack = false;

    }

    /*
     * Reenables player input and revives the character with half health
     */
    [ServerRpc (RequireOwnership = false)]
    public void Revive()
    {
        
        ServerHeal();
        ShowHUD(Owner);
        ToggleInput(Owner, true);
        isInvicible = false;
        attacker.canAttack = true;
    }

    [ServerRpc(RequireOwnership = false)]
    private void ServerHeal()
    {
        TakeDamage(baseStats.maxHealth * -0.5f);
    }

    [TargetRpc]
    private void ShowRespawn(NetworkConnection conn)
    {
        UIManager.LocalInstance.Show<Respawn>();
    }

    [TargetRpc]
    private void ShowHUD(NetworkConnection conn)
    {
        UIManager.LocalInstance.Show<GameInfo>();
    }


    [TargetRpc]
    private void ToggleInput(NetworkConnection conn, bool value)
    {
        if (value)
        {
            GetComponent<PlayerInput>().ActivateInput();
        } else
        {
            GetComponent<PlayerInput>().DeactivateInput();
        }
    }

    public void OnPause()
    {
        UIManager.LocalInstance.Show<PauseMenu>();
    }

}