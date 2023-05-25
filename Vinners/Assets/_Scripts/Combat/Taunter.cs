using UnityEngine;

public class Taunter : MonoBehaviour
{
    public GameObject target;
    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyAI enemy = other.gameObject.GetComponent<EnemyAI>();
        if (enemy != null)
        {
            // enemy.SetTarget(target);
        }
    }
}
