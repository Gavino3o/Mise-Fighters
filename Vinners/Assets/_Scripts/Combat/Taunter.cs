using UnityEngine;
using FishNet.Object;

public class Taunter : NetworkBehaviour
{
    public NetworkObject target;
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            // enemy.SetTarget(target);
        }
    }
}
