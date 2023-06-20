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

    public event Action DamageTaken;
    public event Action<float> HealthChanged;
    public event Action<int> StatusApplied;
    public event Action<int> StatusEnded;

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

        DamageTaken?.Invoke();
        HealthChanged?.Invoke(next);        
    }

    public abstract void OnDeath();

    // Keeps track of changes to individual stats
    public float attackChangeEndtime;
    public float speedChangeEndtime;
    public float dousedUntil;
    public bool isDoused;
    public bool isPoisoned;

    // Keeps track of StatusEffect endtimes for animation purposes
    private readonly float[] statusEndtimes = new float[8];

    public void ApplyStatusEffect(StatusEffectData sed)
    {
        if (sed.attackMultiplier != 1)
        {
            AlterAttack(sed.attackMultiplier, sed.durationSeconds);
        }

        if (sed.moveSpeedMultiplier != 1)
        {
            AlterSpeed(sed.moveSpeedMultiplier, sed.durationSeconds);
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

        StatusApplied?.Invoke(sed.effectCode);
        statusEndtimes[sed.effectCode] = Time.time + sed.durationSeconds;
    }


    private void Update()
    {
        if (attackChangeEndtime <= Time.time) currAttack = baseStats.attack;
        if (speedChangeEndtime <= Time.time) currMoveSpeed = baseStats.moveSpeed;
        if (dousedUntil <= Time.time) isDoused = false;

        for (int n = 0; n < statusEndtimes.Length; n++)
        {
            if (statusEndtimes[n] <= Time.time) StatusEnded?.Invoke(n);
        }
    }


    /*
     * TODO: Handle stacking of different calue stat changes that are tied into different status effects..
     */
    #region Status Effect Methods
    public void Douse(float duration)
    {
        dousedUntil = Time.time + duration;
        isDoused = true;
    }

    public void AlterAttack(float multiplier, float duration)
    {
        float next = multiplier * baseStats.attack;
        if (next < currAttack)
        {
            currAttack = next;
        }

        attackChangeEndtime = Time.time + duration;
    }

    public void AlterSpeed(float multiplier, float duration)
    {
        float next = multiplier * baseStats.moveSpeed;
        if (next < currMoveSpeed)
        {
            currMoveSpeed = next;
        }
        speedChangeEndtime = Time.time + duration;
    }

    public IEnumerator Dot(float dmg, float duration)
    {
        if (isPoisoned) yield return null;
        isPoisoned = true;
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            TakeDamage(dmg);
            yield return new WaitForSeconds(0.5f);
        }
        isPoisoned = false;
    }

    #endregion

}
