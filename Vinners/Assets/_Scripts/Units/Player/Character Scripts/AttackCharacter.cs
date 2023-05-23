using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class AttackCharacter : NetworkBehaviour
{
    private Character character;
    private Rigidbody2D rigidBody;
    private InputCharacter input;

    [SerializeField] private GameObject projectile;

    private float lastAttacked;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        input = GetComponent<InputCharacter>();
        rigidBody = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
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
     * 
     * Notes: Instantiate with transform will parent object (the damager will follow player movement) (for melee attacks) but
     * Instantiate with position and rotation just spawns it from the player position (for projectiles i guess)
     * 
     * Or should there be no distinction for both?
     */
    [ServerRpc]
    public void AutoAttack()
    {
        Debug.Log($"{gameObject} controlled by {Owner} attacks!");
        GameObject obj = Instantiate(projectile, gameObject.transform.position, gameObject.transform.rotation);
        ServerManager.Spawn(obj);       
    }


}
