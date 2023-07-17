using System.Collections;
using UnityEngine;
using FishNet.Object;

public class ButcherCastCharacter : CastCharacter
{
    # region Taunt skill
    [Header("Taunt Skill")]
    [SerializeField] private NetworkObject tauntSpellPrefab;
    [SerializeField] private AudioClip skillSpellSoundEffect;
    [SerializeField] private AudioClip dashSpellSoundEffect;
    [SerializeField] private AudioClip ultimateSpellSoundEffect;
    public void OnSkill()
    {
        if (!IsOwner) return;
        if (base.canCast[0])
        {
            StartCoroutine(Cooldown(0));
            characterAnimator.PlaySkill();
            CastTauntSkill();
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    [ServerRpc]
    public void CastTauntSkill()
    {
        NetworkObject obj = Instantiate(tauntSpellPrefab, transform);
        SetupDamager(obj.GetComponent<EnemyDamager>(), 0);
        obj.GetComponent<Lifetime>().lifetime = spellData[0].duration;
        obj.GetComponent<Taunter>().target = gameObject;
        ServerManager.Spawn(obj);
        AudioManager.Instance.PlaySoundEffect(skillSpellSoundEffect);
        Debug.Log($"{spellData[0].spellName} casted");
    }

    #endregion

    # region Charge skill
    [Header("Charge Skill")]
    public float chargeSpeed = 3f;
    public float windUp = 0.3f;
    [SerializeField] private NetworkObject chargeSpellPrefab;

    public void OnDash()
    {
        if (!IsOwner) return;
        if (canCast[1])
        {
            StartCoroutine(Cooldown(1));
            characterAnimator.PlayDash(spellData[1].duration);
            CastChargeSkill();
            StartCoroutine(Charge());
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    // spawn a damager on the player to do the aoe for windup + lifetime duration + 0.2 (satisfaction time)
    [ServerRpc]
    public void CastChargeSkill()
    {
        NetworkObject obj = Instantiate(chargeSpellPrefab, transform);
        SetupDamager(obj.GetComponent<EnemyDamager>(), 1);
        obj.GetComponent<Lifetime>().lifetime = spellData[1].duration;
        ServerManager.Spawn(obj);
        AudioManager.Instance.PlaySoundEffect(dashSpellSoundEffect);
        Debug.Log($"{spellData[1].spellName} casted");
    }

    public IEnumerator Charge()
    {
        movement.interrupted = true;
        character.isInvicible = true;
        yield return new WaitForSeconds(windUp);
        rigidBody.velocity = 3 * chargeSpeed * input.targetDirection;
        yield return new WaitForSeconds(spellData[1].duration);
        movement.interrupted = false;
        character.isInvicible = false;
    }

    #endregion

    #region Ultimate skill

    [Header("Ultimate Skill")]
    public int noSpins = 5;
    public float spinInterval = 0.5f;
    [SerializeField] private NetworkObject spinSpellPrefab;
    public void OnUltimate()
    {
        if (!IsOwner) return;
        if (canCast[2])
        {
            StartCoroutine(Pirouette());
            characterAnimator.PlayUltimate(noSpins * spinInterval);
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
        NetworkObject obj = Instantiate(spinSpellPrefab, transform);
        SetupDamager(obj.GetComponent<EnemyDamager>(), 2);
        obj.GetComponent<Lifetime>().lifetime = spellData[2].duration;
        ServerManager.Spawn(obj);
        Debug.Log($"{spellData[2].spellName} casted");
    }

    public IEnumerator Pirouette()
    {
        int n = 0;
        AudioManager.Instance.PlaySoundEffect(ultimateSpellSoundEffect);
        while (n < noSpins)
        {
            CastUltimateSkill();
            n++;
            yield return new WaitForSeconds(spinInterval);
        }
    }
    #endregion
}
