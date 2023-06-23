using System.Collections;
using UnityEngine;
using FishNet.Object;
public class BartenderCastCharacter : CastCharacter
{
    #region Bomb skill
    [Header("Bomb Skill")]
    [SerializeField] private NetworkObject bombSpellPrefab;

    public void OnSkill()
    {
        if (!IsOwner) return;
        if (base.canCast[0])
        {
            StartCoroutine(Cooldown(0));
            characterAnimator.PlaySkill();
            CastBombSkill(input.mousePos);
            Debug.Log("Spell casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    [ServerRpc]
    public void CastBombSkill(Vector2 mousePos)
    {
        NetworkObject obj = Instantiate(bombSpellPrefab, mousePos, transform.rotation);
        SetupDamager(obj.GetComponent<EnemyDamager>(), 0);
        obj.GetComponent<Lifetime>().lifetime = spellData[0].duration;
        ServerManager.Spawn(obj);
        Debug.Log($"{spellData[0].spellName} casted");
    }

    #endregion

    #region Backstep skill
    [Header("Backstep Skill")]
    public float dashSpeed = 6f;
    [SerializeField] private NetworkObject lurePrefab;
    public float lureDuration = 2f;
    public void OnDash()
    {
        if (!IsOwner) return;
        if (canCast[1])
        {           
            DropLure();
            StartCoroutine(Cooldown(1));
            characterAnimator.PlayDash();
            StartCoroutine(Backstep());
            Debug.Log($"{spellData[1].spellName} casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    [ServerRpc]
    public void DropLure()
    {
        NetworkObject obj = Instantiate(lurePrefab, transform.position, transform.rotation);
        obj.GetComponent<Lifetime>().lifetime = lureDuration;
        obj.GetComponent<Taunter>().target = obj;
        ServerManager.Spawn(obj);
        Debug.Log("Lure dropped");
    }

    public IEnumerator Backstep()
    {
        movement.interrupted = true;
        rigidBody.velocity = -3 * dashSpeed * input.targetDirection;
        yield return new WaitForSeconds(spellData[1].duration);
        movement.interrupted = false;
    }
    #endregion

    #region Ultimate skill

    [Header("Ultimate Skill")]
    [SerializeField] private NetworkObject iceSpellPrefab;
    public void OnUltimate()
    {
        if (!IsOwner) return;
        if (canCast[2])
        {
            CastUltimateSkill();
            characterAnimator.PlayUltimate();
            SpendUltimate(ULT_METER);
        }
        else
        {
            Debug.Log("Not enough charge");
        }
    }

    [ServerRpc]
    public void CastUltimateSkill()
    {
        NetworkObject obj = Instantiate(iceSpellPrefab, transform.position, transform.rotation);
        SetupDamager(obj.GetComponent<EnemyDamager>(), 2);
        obj.GetComponent<Lifetime>().lifetime = spellData[2].duration;
        ServerManager.Spawn(obj);
        Debug.Log($"{spellData[2].spellName} casted");
    }

    #endregion
}
