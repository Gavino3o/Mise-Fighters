using UnityEngine;

// Should separate into Character and Enemy status appliers
public class StatusApplier : MonoBehaviour
{
    [SerializeField] private StatusEffectData[] sed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Unit unit = other.gameObject.GetComponent<Unit>();
        if (unit != null)
        {
            foreach(StatusEffectData effect in sed)
            {
                unit.ApplyStatusEffect(effect);
                Debug.Log($"Status effect {effect.effectName} applied to {other.gameObject}!");
            }           
        }
    }

}
