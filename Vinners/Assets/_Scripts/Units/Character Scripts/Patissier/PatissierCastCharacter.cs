using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using UnityEngine.InputSystem;

public class PatissierCastCharacter : CastCharacter 
{
    #region Burn skill
    [Header("Burn Skill")]
    [SerializeField] private GameObject burnSpellPrefab;
    public void OnSkill()
    {
        if (!IsOwner) return;
        if (base.canCast[0])
        {
            StartCoroutine(Cooldown(0));

            CastBurnSkill();
            Debug.Log("Spell casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    [ServerRpc]
    public void CastBurnSkill()
    {
        var direction = input.targetDirection;
        GameObject obj = null;
        
        if (direction.x < transform.position.x)
        {
            // If mouse input is on the left
            obj = Instantiate(burnSpellPrefab, transform.right, Quaternion.Euler(0, 0, 90));
            obj.GetComponent<SkillFollowPlayer>().direction = 3;
        }
        else 
        {
            // If mouse input is on the right, exact above or exact below
            obj = Instantiate(burnSpellPrefab, -transform.right, Quaternion.Euler(0, 0, -90));
            obj.GetComponent<SkillFollowPlayer>().direction = 1;
        }

        obj.GetComponent<SkillFollowPlayer>().player = gameObject;
        obj.GetComponent<EnemyDamager>().damage = spellData[0].damage * character.currAttack;
        obj.GetComponent<Lifetime>().lifetime = spellData[0].duration;
        ServerManager.Spawn(obj);
        Debug.Log($"{spellData[0].spellName} casted");
    }

    #endregion


    #region Scramble skill
    [Header("Charge Skill")]
    public float scrambleSpeed = 8f;
    public void OnDash()
    {
        if (!IsOwner) return;
        if (canCast[1])
        {
            StartCoroutine(Cooldown(1));
            // CastChargeSkill();
            StartCoroutine(Scramble());
            Debug.Log($"{spellData[1].spellName} casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    public IEnumerator Scramble()
    {
        movement.interrupted = true;
        rigidBody.velocity = scrambleSpeed * input.targetDirection;
        yield return new WaitForSeconds(spellData[1].duration);
        movement.interrupted = false;
    }

    #endregion
}
