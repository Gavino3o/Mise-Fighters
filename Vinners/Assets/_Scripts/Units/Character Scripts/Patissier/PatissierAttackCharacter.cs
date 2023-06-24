using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatissierAttackCharacter : AttackCharacter
{
    private Vector2[] cardinalDirections = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

    private void Awake()
    {
        character = GetComponent<Character>();
        input = character.input;
        canAttack = true;
        projectile.source = character;
    }

    private void Update()
    {
        if (!IsOwner) return;
        if (!canAttack) return;
        if (character == null || input == null || input.targetDirection == null) return;

        if (Time.time - lastAttacked < character.currAttackSpeed) return;
        lastAttacked = Time.time;
        // Values have to be calculated/accessed outside the serverrpc call
        AutoAttack(character.currAttack, input.targetDirection, input.rotation);
    }

    [ServerRpc]
    public override void AutoAttack(float attack, Vector2 targetDirection, Quaternion rotation)
    {
        foreach (var direction in cardinalDirections)
        {
            GameObject obj = Instantiate(projectile.gameObject, transform.position, rotation);
            EnemyDamager dmger = obj.GetComponent<EnemyDamager>();
            if (dmger != null) dmger.damage = attack;

            SkillshotMotion motion = obj.GetComponent<SkillshotMotion>();
            if (motion != null) motion.movementDirection = direction;

            ServerManager.Spawn(obj);  
        }
        AudioManager.Instance.PlaySoundEffect(autoAttackSoundEffect);
        Debug.Log($"{gameObject} controlled by {Owner} attacks!");
    }
}
