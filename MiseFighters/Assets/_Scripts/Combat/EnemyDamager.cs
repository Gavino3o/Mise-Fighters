using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    public Character source;
    public float damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyAI unit = other.gameObject.GetComponent<EnemyAI>();
        if (unit != null)
        {
            unit.TakeDamage(damage);
            Debug.Log($"{gameObject} dealt {damage} damage to {other.gameObject}!");
            if (source != null)
            {
                source.HitSuccess();
            }
            else
            {
                Debug.Log("Source is null");
            }

        }
    
    }

}
