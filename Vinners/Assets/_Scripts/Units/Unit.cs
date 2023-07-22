using System.Collections;
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

    public UnitStats baseStats;

    public event Action DamageTaken;
    public event Action<float> HealthChanged;
    public event Action<int> StatusApplied;
    public event Action<int> StatusEnded;

    [SyncVar] public bool isInvicible;

    public SpriteRenderer sprite;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        currHealth = baseStats.maxHealth;
        currAttack = baseStats.attack;
        currAttackSpeed = baseStats.attackFreqSeconds;
        currMoveSpeed = baseStats.moveSpeed;
    }

    // This should be the only way a unit's health is changed
    public virtual void TakeDamage(float dmg)
    {
        if (isInvicible) return;
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
            statusEndtimes[(int) StatusEffectData.EFFECTCODES.DOUSE] = Time.time;
        }

        if (sed.effectCode <= -1) return;

        StatusApplied?.Invoke(sed.effectCode);
        AnimateStatus(sed.effectCode);
        statusEndtimes[sed.effectCode] = Time.time + sed.durationSeconds;
    }

    private void AnimateStatus(int effectcode)
    {
        switch (effectcode)
        {
            case (int) StatusEffectData.EFFECTCODES.SLOW:
                sprite.color = Color.cyan;
                sprite.flipY = false;
                break;
            case (int)StatusEffectData.EFFECTCODES.FREEZE:
                sprite.color = Color.blue;
                sprite.flipY = false;
                break;
            case (int)StatusEffectData.EFFECTCODES.DOUSE:
                sprite.color = Color.gray;
                sprite.flipY = false;
                break;
            case (int)StatusEffectData.EFFECTCODES.BURN:
                sprite.color = Color.red;
                sprite.flipY = false;
                break;
            case (int)StatusEffectData.EFFECTCODES.BUFF:
                sprite.color = new(0.5f, 1, 0.5f);
                sprite.flipY = false;
                break;
            case (int)StatusEffectData.EFFECTCODES.FLATTEN:
                sprite.color = Color.white;
                sprite.flipY = true;
                break;
            default:
                break;
        }
    }

    private void EndAnimateStatus()
    {
        for (int n = 0; n < statusEndtimes.Length; n++)
        {
            if (statusEndtimes[n] > Time.time)
            {
                AnimateStatus(n);
                return;
            }
        }
        sprite.color = Color.white;
        sprite.flipY = false;
    }

    private void Update()
    {
        if (attackChangeEndtime <= Time.time) currAttack = baseStats.attack;
        if (speedChangeEndtime <= Time.time) currMoveSpeed = baseStats.moveSpeed;
        if (dousedUntil <= Time.time) isDoused = false;

        for (int n = 0; n < statusEndtimes.Length; n++)
        {
            if (statusEndtimes[n] <= Time.time)
            {
                EndAnimateStatus();
                StatusEnded?.Invoke(n);
            }
        }
    }


    /*
     * Current behaviour: Stat changes override any previous ones, DoT is stackable.
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
        currAttack = next;
        

        attackChangeEndtime = Time.time + duration;
    }

    public void AlterSpeed(float multiplier, float duration)
    {
        float next = multiplier * baseStats.moveSpeed;
        currMoveSpeed = next;

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
