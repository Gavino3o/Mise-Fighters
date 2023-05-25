using System.Collections;
using UnityEngine;
using FishNet.Object;
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
            CastBombSkill(spellData[0].duration, spellData[0].damage * character.currAttack, new Vector3(input.mousePos.x, input.mousePos.y, 0f));
            Debug.Log("Spell casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    [ServerRpc]
    public void CastBombSkill(float duration, float dmg, Vector3 position)
    {
        GameObject obj = Instantiate(bombSpellPrefab, position, transform.rotation);
        obj.GetComponent<Lifetime>().lifetime = duration;
        obj.GetComponent<EnemyDamager>().damage = dmg;
        ServerManager.Spawn(obj);
        Debug.Log($"{spellData[0].spellName} casted");
    }

    #endregion

    #region Backstep skill
    [Header("Backstep Skill")]
    public float dashSpeed = 6f;
    [SerializeField] private GameObject lurePrefab;
    public float lureDuration = 2f;
    public void OnDash()
    {
        if (!IsOwner) return;
        if (canCast[1])
        {
            DropLure(lureDuration);
            StartCoroutine(Cooldown(1));
            
            StartCoroutine(Charge());
            Debug.Log($"{spellData[1].spellName} casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    [ServerRpc]
    public void DropLure(float lureDuration)
    {
        GameObject obj = Instantiate(lurePrefab, transform.position, transform.rotation);
        obj.GetComponent<Lifetime>().lifetime = lureDuration;
        ServerManager.Spawn(obj);
        Debug.Log("Lure dropped");
    }

    public IEnumerator Charge()
    {
        movement.interrupted = true;
        rigidBody.velocity = -3 * dashSpeed * input.targetDirection;
        yield return new WaitForSeconds(spellData[1].duration);
        movement.interrupted = false;
    }
    #endregion
}
