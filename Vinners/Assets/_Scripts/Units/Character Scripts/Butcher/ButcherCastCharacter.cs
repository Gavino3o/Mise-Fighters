using System.Collections;
using UnityEngine;
using FishNet.Object;

public class ButcherCastCharacter : CastCharacter
{
    # region Taunt skill
    [Header("Taunt Skill")]
    [SerializeField] private GameObject tauntSpellPrefab;
    public void OnSkill()
    {
        if (!IsOwner) return;
        if (base.canCast[0])
        {
            StartCoroutine(Cooldown(0));
            
            CastTauntSkill();
            Debug.Log("Spell casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    [ServerRpc]
    public void CastTauntSkill()
    {
        GameObject obj = Instantiate(tauntSpellPrefab, transform);
        obj.GetComponent<EnemyDamager>().damage = spellData[0].damage * character.currAttack;
        obj.GetComponent<Lifetime>().lifetime = spellData[0].duration;
        obj.GetComponent<Taunter>().target = NetworkObject;
        ServerManager.Spawn(obj);
        Debug.Log($"{spellData[0].spellName} casted");
    }

    #endregion

    # region Charge skill
    [Header("Charge Skill")]
    public float chargeSpeed = 3f;
    public float windUp = 0.3f;
    public void OnDash()
    {
        if (!IsOwner) return;
        if (canCast[1])
        {
            StartCoroutine(Cooldown(1));
            // CastChargeSkill();
            StartCoroutine(Charge());
            Debug.Log($"{spellData[1].spellName} casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    [ServerRpc]
    public void CastChargeSkill()
    {
        // spawn a damager on the player to do the aoe for windup + lifetime duration + 0.2 (satisfaction time)
    }

    public IEnumerator Charge()
    {
        movement.interrupted = true;
        yield return new WaitForSeconds(windUp);
        rigidBody.velocity = 3 * chargeSpeed * input.targetDirection;
        yield return new WaitForSeconds(spellData[1].duration);
        movement.interrupted = false;
    }

    #endregion

    #region Ultimate skill
    public void OnUltimate()
    {
        if (!IsOwner) return;
        if (canCast[2])
        {
            Debug.Log($"Ultimate casted");
            SpendUltimate(ULT_METER);
        }
        else
        {
            Debug.Log("Not enough charge");
        }
    }
    #endregion
}
