using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using System;

// Maybe use arrays and an enum
public abstract class Unit : NetworkBehaviour
{
    [SyncVar] public float currHealth;
    [SyncVar] public float currAttack;
    [SyncVar] public float currAttackSpeed;
    [SyncVar] public float currMoveSpeed;

    [SerializeField] protected UnitStats baseStats;


    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        currHealth = baseStats.maxHealth;
        currAttack = baseStats.attack;
        currAttackSpeed = baseStats.attackFreqSeconds;
        currMoveSpeed = baseStats.moveSpeed;
    }

    // This should be the only way a unit's health is changed
    public void TakeDamage(float dmg)
    {
        float next = currHealth -= dmg;
        if (next >= baseStats.maxHealth)
        {
            currHealth = baseStats.maxHealth;
        }
        else
        {
            currHealth = Mathf.Max(next, 0f);
        }
        if (currHealth <= 0)
        {
            OnDeath();
        }
    }

    public abstract void OnDeath();

    // public for debugging purposes
    public float crippledUntil;
    public float slowedUntil;
    public float dousedUntil;
    public bool isDoused;

    public void ApplyStatusEffect(StatusEffectData sed)
    {
        if (sed.attackMultiplier != 1)
        {
            Cripple(sed.attackMultiplier, sed.durationSeconds);
        }

        if (sed.moveSpeedMultiplier != 1)
        {
            Slow(sed.moveSpeedMultiplier, sed.durationSeconds);
        }


        if (sed.damageOverTime != 0)
        {
            StartCoroutine(Dot(sed.damageOverTime, sed.durationSeconds));
        }

        if (sed.douser)
        {
           Douse(sed.durationSeconds);

        }

        if (sed.igniter && isDoused)
        {
            TakeDamage(StatusEffectData.IGNITION_DMG);
            dousedUntil = Time.time;
        }
    }


    private void Update()
    {
        if (crippledUntil <= Time.time) currAttack = baseStats.attack;
        if (slowedUntil <= Time.time) currMoveSpeed = baseStats.moveSpeed;
        if (dousedUntil <= Time.time) isDoused = false;
    }
    #region Status Effect Methods
    public void Douse(float duration)
    {
        dousedUntil = Time.time + duration;
        isDoused = true;
    }

    public void Cripple(float multiplier, float duration)
    {
        float next = multiplier * baseStats.attack;
        if (next < currAttack)
        {
            currAttack = next;
        }

        crippledUntil = Time.time + duration;
    }

    public void Slow(float multiplier, float duration)
    {
        float next = multiplier * baseStats.moveSpeed;
        if (next < currMoveSpeed)
        {
            currMoveSpeed = next;
        }
        slowedUntil = Time.time + duration;
    }

    public IEnumerator Dot(float dmg, float duration)
    {
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            TakeDamage(dmg);
            yield return new WaitForSeconds(0.5f);
        }
    }

    #endregion

}
