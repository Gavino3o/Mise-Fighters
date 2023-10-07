using UnityEngine;
using FishNet.Object;

public class Taunter : NetworkBehaviour
{
    public GameObject target;
    public float tauntDuration = 4f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            enemy.TauntedBy(target, tauntDuration);
        }
    }
}
