using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using System;
using UnityEngine.InputSystem;

public class ButcherCastCharacter : NetworkBehaviour
{
    private InputCharacter input;
    private Character character;
    private Rigidbody2D rigidBody;
    private MoveCharacter movement;

    public SpellData[] spellData = new SpellData[3];
    private readonly bool[] canCast = new bool[3];

    public override void OnStartClient()
    {
        base.OnStartClient();
        character = GetComponent<Character>();
        input = GetComponent<InputCharacter>();
        rigidBody = GetComponent<Rigidbody2D>();
        movement = GetComponent<MoveCharacter>();

        Array.Fill(canCast, true);
    }

    # region Taunt skill
    [Header("Taunt Skill")]
    [SerializeField] private GameObject tauntSpellPrefab;
    public void OnSkill()
    {
        if (!IsOwner) return;
        CastTauntSkill();
    }

    [ServerRpc]
    public void CastTauntSkill()
    {
        if (canCast[0])
        {
            StartCoroutine(Cooldown(0));
            GameObject obj = Instantiate(tauntSpellPrefab, transform);
            obj.GetComponent<CharacterDamager>().lifetime = spellData[0].lifetime;
            obj.GetComponent<CharacterDamager>().damage = spellData[0].damage * character.currAttack;
            ServerManager.Spawn(obj);
            Debug.Log("Spell casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    #endregion

    # region Charge skill
    [Header("Charge Skill")]
    public float chargeSpeed = 3f;
    public float chargeTime = 0.8f;
    public float windUp = 0.3f;
    public void OnDash()
    {
        if (!IsOwner) return;
        CastChargeSkill();
    }

    [ServerRpc]
    public void CastChargeSkill()
    {
        if (canCast[1])
        {
            StartCoroutine(Cooldown(1));
            ObserverCastCharge();
            Debug.Log("Charge spell casted");
        }
        else
        {
            Debug.Log("Spell is on cooldown");
        }
    }

    [ObserversRpc]
    private void ObserverCastCharge()
    {
        StartCoroutine(Charge());
    }

    public IEnumerator Charge()
    {
        
        movement.interrupted = true;
        yield return new WaitForSeconds(windUp);
        rigidBody.velocity = 3 * chargeSpeed * input.targetDirection;
        yield return new WaitForSeconds(chargeTime);
        movement.interrupted = false;
    }

    #endregion

    IEnumerator Cooldown(int id)
    {
        canCast[id] = false;
        yield return new WaitForSeconds(spellData[id].cooldown);
        canCast[id] = true;
    }


}
