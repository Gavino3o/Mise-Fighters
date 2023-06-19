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

        }
        // placed here for now, should be inside if bracket
        if (source != null) source.HitSuccess();
    }

}
