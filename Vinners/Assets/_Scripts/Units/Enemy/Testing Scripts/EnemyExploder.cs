using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class EnemyExploder : NetworkBehaviour
{
    //Note: explodingCancelRange must be >= exploding Range
    [SerializeField] private AudioClip explodingAudioClip;
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
        }
        else
        {
            Explode();
        }
    }

    [ServerRpc] 
    private void Explode()
    {
        var explosion = Instantiate(explosionPrefab, gameObject.transform.position, Quaternion.identity);
        ServerManager.Spawn(explosion);
        // Insert explosion animation and audio here
        enemyAI.OnDeath();
    }

    private bool PlayerExceedCancelRange()
    {
        var playerTransform = playerTargeter.GetCurrentTargetPlayer().transform.position;
        return Vector2.Distance(transform.position, playerTransform) > explodingCancelRange;
    }
}
