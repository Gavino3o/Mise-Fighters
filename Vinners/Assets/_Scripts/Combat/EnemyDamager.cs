using UnityEngine;

public class EnemyDamager : MonoBehaviour
{
    public float damage;
    public float lifetime;

    private void Update()
    {
        if (lifetime <= 0) Destroy(gameObject);

        lifetime -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        /*
        Enemy unit = other.gameObject.GetComponent<Enemy>();
        if (unit != null)
        {
            unit.TakeDamage(damage);
            Debug.Log($"{gameObject} dealt {damage} damage to {other.gameObject}!");
        }
        */
    }
}
