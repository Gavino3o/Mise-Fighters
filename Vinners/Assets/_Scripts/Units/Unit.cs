using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

// Maybe use arrays and an enum
public abstract class Unit : NetworkBehaviour
{
    [SyncVar] public float currHealth;
    [SyncVar] public float currAttack;
    [SyncVar] public float currAttackSpeed;
    [SyncVar] public float currMoveSpeed;

    [SerializeField] protected UnitStats baseStats;

    private bool isSlowed;
    private bool isDoused;
    private bool isCrippled;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        currHealth = baseStats.maxHealth;
        currAttack = baseStats.attack;
        currAttackSpeed = baseStats.attackFreqSeconds;
        currMoveSpeed = baseStats.moveSpeed;
    }

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
    }

    // Stacking status effects breaks this, have to rewrite.
    public void ApplyStatusEffect(StatusEffectData sed)
    {
        StartCoroutine(Cripple(sed.attackMultiplier, sed.durationSeconds));
        StartCoroutine(Slow(sed.moveSpeedMultiplier, sed.durationSeconds));
        StartCoroutine(Dot(sed.damageOverTime, sed.durationSeconds));
    }

    #region Status Effect Coroutines
    public IEnumerator Cripple(float multiplier, float duration)
    {
        float original = currAttack;
        currAttack *= multiplier;
        yield return new WaitForSeconds(duration);
        currAttack = original;
    }

    // Just temporary to make sure slows don't stack and never revert. Need to rewrite to make sure
    // status stacking lengthens duration + overwrites current multiplier iff it's a stronger variant.
    public IEnumerator Slow(float multiplier, float duration)
    {
        if (isSlowed) yield break;
        isSlowed = true;
        float original = currMoveSpeed;
        currMoveSpeed *= multiplier;
        yield return new WaitForSeconds(duration);
        isSlowed = false;
        currMoveSpeed = original;
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
