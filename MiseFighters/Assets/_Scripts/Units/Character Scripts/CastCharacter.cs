using System.Collections;
using UnityEngine;
using FishNet.Object;
using System;

public class CastCharacter : NetworkBehaviour
{
    protected InputCharacter input;
    protected Character character;
    protected Rigidbody2D rigidBody;
    protected MoveCharacter movement;
    protected AnimatorCharacter characterAnimator;

    public SpellData[] spellData = new SpellData[3];
    public readonly bool[] canCast = new bool[3];

    public static float ULT_METER = 50f;   
    public float ultimate;

    private void Awake()
    {
        character = GetComponent<Character>();

        character.HitEnemy += CharacterHitEnemy;
        character.DamageTaken += CharacterTookDamage;

        input = character.input; 
        rigidBody = character.rb;
        movement = character.movement;
        characterAnimator = character.characterAnimator;
        // for quick testing
        ultimate = 50f;

        Array.Fill(canCast, true);
        CheckUltimate();
    }

    private void OnDestroy()
    {
        character.HitEnemy -= CharacterHitEnemy;
        character.DamageTaken -= CharacterTookDamage;
    }

    public IEnumerator Cooldown(int id)
    {
        canCast[id] = false;
        yield return new WaitForSeconds(spellData[id].cooldown);
        canCast[id] = true;
    }

    private void CharacterHitEnemy()
    {
        ChargeUltimate(0.5f);
    }

    private void CharacterTookDamage()
    {
        ChargeUltimate(0.25f);
    }

    public void ChargeUltimate(float amt)
    {
        ultimate = Math.Min(ultimate + amt, ULT_METER);
        CheckUltimate();
    }

    public void SpendUltimate(float amt)
    {
        ultimate = Math.Max(0, ultimate - amt);
        CheckUltimate();
    }

    private void CheckUltimate()
    {
        if (ultimate >= ULT_METER)
        {
            canCast[2] = true;
        }
        else if (ultimate < ULT_METER)
        {
            canCast[2] = false;
        }

    }

    #region Helper methods
    protected void SetupDamager(EnemyDamager dmger, int n)
    {
        if (dmger != null)
        {
            dmger.damage = spellData[n].damage * character.currAttack;
            dmger.source = character;
        }
    }
    #endregion
}
