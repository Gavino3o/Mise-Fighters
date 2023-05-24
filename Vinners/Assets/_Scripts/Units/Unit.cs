using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

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

    public IEnumerator Slow(float multiplier, float duration)
    {
        float original = currMoveSpeed;
        currMoveSpeed *= multiplier;
        yield return new WaitForSeconds(duration);
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
