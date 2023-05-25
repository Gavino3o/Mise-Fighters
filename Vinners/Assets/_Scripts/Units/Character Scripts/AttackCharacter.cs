using UnityEngine;
using FishNet.Object;

public class AttackCharacter : NetworkBehaviour
{
    public Character character;
    public InputCharacter input;

    public GameObject projectile;

    private float lastAttacked;

    public override void OnStartClient()
    {
        base.OnStartClient();
        character = GetComponent<Character>();
        input = GetComponent<InputCharacter>();
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (Time.time - lastAttacked < character.currAttackSpeed) return;
        lastAttacked = Time.time;
        // Values have to be calculated/accessed outside the serverrpc call
        if (input.targetDirection != null && input.targetDirection != Vector2.zero) AutoAttack(input.targetDirection);
        
    }

    /*
     * Performs an attack over the server
     * Instantiates the prefab at the current position and sets its parameters.
     */
    [ServerRpc]
    public void AutoAttack(Vector2 targetDirection)
    {
        GameObject obj = Instantiate(projectile, transform.position, transform.rotation);
        EnemyDamager dmger = obj.GetComponent<EnemyDamager>();
        if (dmger != null)
        {
            dmger.damage = character.currAttack;
        }

        SkillshotMotion motion = obj.GetComponent<SkillshotMotion>();
        if (motion != null)
        {
            motion.movementDirection = targetDirection;
        }
        ServerManager.Spawn(obj);
        Debug.Log($"{gameObject} controlled by {Owner} attacks!");
    }


}
