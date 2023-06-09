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

    // Stacking status effects breaks this, have to rewrite.

    private bool isDoused;

    public void ApplyStatusEffect(StatusEffectData sed)
    {
        if (sed.attackMultiplier != 1)
        {
            StartCoroutine(Cripple(sed.attackMultiplier, sed.durationSeconds));
        }

        if (sed.moveSpeedMultiplier != 1)
        {
            StartCoroutine(Slow(sed.moveSpeedMultiplier, sed.durationSeconds));
        }


        if (sed.damageOverTime != 0)
        {
            StartCoroutine(Dot(sed.damageOverTime, sed.durationSeconds));
        }

        if (sed.douser)
        {
            StartCoroutine(Douse(sed.durationSeconds));

        }

        if (sed.igniter && isDoused)
        {
            TakeDamage(StatusEffectData.IGNITION_DMG);
            isDoused = false;
        }
    }

    #region Status Effect Coroutines
    public IEnumerator Douse(float duration)
    {
        if (!isDoused)
        {
            isDoused = true;
            yield return new WaitForSeconds(duration);
            isDoused = false;
        }
        else
        {
            yield return null;
        }
    }

    public IEnumerator Cripple(float multiplier, float duration)
    {
        float next = multiplier * baseStats.attack;
        if (next < currAttack)
        {
            currAttack = next;
            yield return new WaitForSeconds(duration);
            currAttack = baseStats.attack;
        } else
        {
            yield return null;
        }

    }

    // Just temporary to make sure slows don't stack and never revert. Need to rewrite to make sure
    // status stacking lengthens duration + overwrites current multiplier iff it's a stronger variant.
    public IEnumerator Slow(float multiplier, float duration)
    {
        float next = multiplier * baseStats.moveSpeed;
        if (next < currAttack)
        {
            currMoveSpeed= next;
            yield return new WaitForSeconds(duration);
            currMoveSpeed = baseStats.moveSpeed;
        }
        else
        {
            yield return null;
        }

    }

    // DoTs are rare and should be stackable?
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
