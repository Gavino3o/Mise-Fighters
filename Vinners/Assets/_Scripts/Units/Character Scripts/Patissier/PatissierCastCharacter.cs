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
    [SerializeField] private float offSet = 1.5f;
    [SerializeField] private AudioClip skillSpellSoundEffect;
    [SerializeField] private AudioClip dashSpellSoundEffect;
    [SerializeField] private AudioClip ultimateSpellSoundEffect;

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
        var mousePosition = input.mousePos;
        
        if (mousePosition.x < transform.position.x)
        {
            // If mouse input is on the left
            GameObject obj = Instantiate(burnSpellPrefab, transform.right, Quaternion.Euler(0, 0, 270));
            Debug.Log("Left");
            var skillFollowPlayer = obj.GetComponent<SkillFollowPlayer>();
            skillFollowPlayer.player = gameObject;
            skillFollowPlayer.xOffset = -offSet;
            obj.GetComponent<EnemyDamager>().damage = spellData[0].damage * character.currAttack * 0.5f * 8;
            obj.GetComponent<Lifetime>().lifetime = spellData[0].duration;
            ServerManager.Spawn(obj);
        }
        else 
        {
            // If mouse input is on the right, exact above or exact below
            GameObject obj = Instantiate(burnSpellPrefab, -transform.right, Quaternion.Euler(0, 0, -270));
            Debug.Log("Right");
            var skillFollowPlayer = obj.GetComponent<SkillFollowPlayer>();
            skillFollowPlayer.player = gameObject;
            skillFollowPlayer.xOffset = offSet;
            obj.GetComponent<EnemyDamager>().damage = spellData[0].damage * character.currAttack * 0.5f * 8;
            obj.GetComponent<Lifetime>().lifetime = spellData[0].duration;
            ServerManager.Spawn(obj);    
        }
        AudioManager.Instance.ObserversPlaySoundEffect(skillSpellSoundEffect);
        Debug.Log($"{spellData[0].spellName} casted");
    }

    #endregion

    #region Scramble skill
    [Header("Scramble Skill")]
    public float scrambleSpeed = 8f;
    public void OnDash()
    {
        if (!IsOwner) return;
        if (canCast[1])
        {
            StartCoroutine(Cooldown(1));
            StartCoroutine(CastScrambleSkill());
            AudioManager.Instance.ObserversPlaySoundEffect(dashSpellSoundEffect);
            Debug.Log($"{spellData[1].spellName} casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    public IEnumerator CastScrambleSkill()
    {
        movement.interrupted = true;
        rigidBody.velocity = scrambleSpeed * input.targetDirection;
        yield return new WaitForSeconds(spellData[1].duration);
        movement.interrupted = false;
    }

    #endregion

    #region Ultimate skill
    [Header("Ultimate skill")]
    [SerializeField] private NetworkObject pinSpellPrefab;
    public void OnUltimate()
    {
        if (!IsOwner) return;
        if (canCast[2])
        {
            CastUltimateSkill();
            AudioManager.Instance.ObserversPlaySoundEffect(ultimateSpellSoundEffect);
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
        NetworkObject obj = Instantiate(pinSpellPrefab, transform.position, input.rotation);
        SkillshotMotion motion = obj.GetComponent<SkillshotMotion>();
        if (motion != null) motion.movementDirection = input.targetDirection;
        SetupDamager(obj.GetComponent<EnemyDamager>(), 2);
        obj.GetComponent<Lifetime>().lifetime = spellData[2].duration;
        ServerManager.Spawn(obj);
        Debug.Log($"{spellData[2].spellName} casted");
    }
    #endregion
}
