using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using System;
using UnityEngine.InputSystem;
public class BartenderCastCharacter : CastCharacter
{
    #region Bomb skill
    [Header("Bomb Skill")]
    [SerializeField] private GameObject bombSpellPrefab;

    public void OnSkill()
    {
        if (!IsOwner) return;
        if (base.canCast[0])
        {
            StartCoroutine(Cooldown(0));
            CastBombSkill();
            Debug.Log("Spell casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    [ServerRpc]
    public void CastBombSkill()
    {
        GameObject obj = Instantiate(bombSpellPrefab, new Vector3(input.mousePos.x, input.mousePos.y, 0f), transform.rotation);
        obj.GetComponent<CharacterDamager>().lifetime = spellData[0].lifetime;
        obj.GetComponent<CharacterDamager>().damage = spellData[0].damage * character.currAttack;
        ServerManager.Spawn(obj);
        Debug.Log("Spell casted");
    }

    #endregion

    #region Backstep skill
    [Header("Backstep Skill")]
    public float dashSpeed = 5f;
    public float chargeTime = 0.3f;
    public void OnDash()
    {
        if (!IsOwner) return;
        if (canCast[1])
        {
            StartCoroutine(Cooldown(1));
            // CastChargeSkill();
            StartCoroutine(Charge());
            Debug.Log("Charge spell casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }
    public IEnumerator Charge()
    {
        movement.interrupted = true;
        rigidBody.velocity = -3 * dashSpeed * input.targetDirection;
        yield return new WaitForSeconds(chargeTime);
        movement.interrupted = false;
    }
    #endregion
}
