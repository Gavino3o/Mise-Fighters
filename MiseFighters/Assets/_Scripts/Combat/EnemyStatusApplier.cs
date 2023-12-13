using UnityEngine;

public class EnemyStatusApplier : MonoBehaviour
{
    [SerializeField] private StatusEffectData[] sed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        EnemyAI unit = other.gameObject.GetComponent<EnemyAI>();
        if (unit != null)
        {
            foreach (StatusEffectData effect in sed)
            {
                unit.ApplyStatusEffect(effect);
                Debug.Log($"Status effect {effect.effectName} applied to {other.gameObject}!");
            }
        }
    }
}
