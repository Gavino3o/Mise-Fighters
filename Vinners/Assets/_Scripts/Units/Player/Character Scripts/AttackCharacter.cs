using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class AttackCharacter : NetworkBehaviour
{
    public Character character;
    private Rigidbody2D rigidBody;
    public InputCharacter input;

    public GameObject projectile;

    private float lastAttacked;

    public override void OnStartClient()
    {
        base.OnStartClient();
        character = GetComponent<Character>();
        input = GetComponent<InputCharacter>();
        rigidBody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (Time.time - lastAttacked < character.currAttackSpeed) return;
        lastAttacked = Time.time;
        AutoAttack();
        
    }

    /*
     * Performs an attack over the server
     * Instantiates the prefab at the current position and sets its parameters.
     */
    [ServerRpc]
    public void AutoAttack()
    {
        GameObject obj = Instantiate(projectile, transform.position, transform.rotation);
        CharacterDamager dmger = obj.GetComponent<CharacterDamager>();
        if (dmger != null)
        {
            dmger.damage = character.currAttack;
        }

        SkillshotMotion motion = obj.GetComponent<SkillshotMotion>();
        if (motion != null)
        {
            motion.movementDirection = input.targetDirection;
            Debug.Log($"direction assigned to {input.targetDirection.x}, {input.targetDirection.y}");
        }
        ServerManager.Spawn(obj);
        Debug.Log($"{gameObject} controlled by {Owner} attacks!");
    }


}
