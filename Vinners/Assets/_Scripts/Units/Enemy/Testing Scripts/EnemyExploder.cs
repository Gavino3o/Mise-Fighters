using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class EnemyExploder : NetworkBehaviour
{
    //Note: explodingCancelRange must be >= exploding Range
    [SerializeField] private AudioClip explosionSoundEffect;
    [SerializeField] private float explodingWindUp;
    [SerializeField] private float explodingCancelRange;
    [SerializeField] private NetworkObject explosionPrefab;

    private PlayerTargeter playerTargeter;
    private EnemyMovementController enemyMovementController;
    private EnemyAI enemyAI;

    private void Start()
    {
        if (!IsServer) return;
        playerTargeter = gameObject.GetComponent<PlayerTargeter>();
        enemyMovementController = gameObject.GetComponent<EnemyMovementController>();
        enemyAI= gameObject.GetComponent<EnemyAI>();
    }

    private void Update()
    {
        if (!IsServer) return;
        if (enemyAI.IsInAttackRange())
        {
            StartCoroutine(ExplodeCoroutine());
        }
    }

    private IEnumerator ExplodeCoroutine()
    {
        enemyMovementController.StopAstarMovement();
        yield return new WaitForSeconds(explodingWindUp); 
        // Insert exploding windup animation and audio here.

        if (PlayerExceedCancelRange())
        {
            // Insert unwind animation here (optional)
            enemyMovementController.StartAstarMovement();
            Debug.Log("Explosion cancelled");
        }
        else
        {
            Explode();
            Debug.Log("Enemy Exploded");
        }

        yield return new WaitForSeconds(1f);
    }

    private void Explode()
    {
        if (!IsServer) return;
        var explosion = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);

        // Hardcoded for testing
        explosion.GetComponent<Lifetime>().lifetime = 1;
        explosion.GetComponent<CharacterDamager>().damage = 1;
        ServerManager.Spawn(explosion);
        // Insert explosion animation here
        AudioManager.Instance.PlaySoundEffect(explosionSoundEffect);
        enemyAI.OnDeath();
    }

    private bool PlayerExceedCancelRange()
    {
        var playerTransform = playerTargeter.GetCurrentTargetPlayer().transform.position;
        return Vector2.Distance(transform.position, playerTransform) > explodingCancelRange;
    }
}
