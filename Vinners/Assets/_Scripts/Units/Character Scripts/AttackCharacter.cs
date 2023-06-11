using UnityEngine;
using FishNet.Object;

public class AttackCharacter : NetworkBehaviour
{
    public Character character;
    public InputCharacter input;

    [SerializeField] private EnemyDamager projectile;

    private float lastAttacked;
    public bool canAttack;

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
        AutoAttack(character.currAttack, input.targetDirection);
        
    }

    /*
     * Performs an attack over the server
     * Instantiates the prefab at the current position and sets its parameters.
     */
    [ServerRpc]
    public void AutoAttack(float attack, Vector2 targetDirection)
    {
        GameObject obj = Instantiate(projectile.gameObject, transform.position, transform.rotation);
        EnemyDamager dmger = obj.GetComponent<EnemyDamager>();
        if (dmger != null)
        {
            dmger.damage = attack;
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