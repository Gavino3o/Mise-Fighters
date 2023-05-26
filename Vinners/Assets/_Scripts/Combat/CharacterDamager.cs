using UnityEngine;

public class CharacterDamager : MonoBehaviour
{ 
    public float damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Character unit = other.gameObject.GetComponent<Character>();
        if (unit != null)
        {
            unit.TakeDamage(damage);
            Debug.Log($"{gameObject} dealt {damage} damage to {other.gameObject}!");
        }
    }

}
