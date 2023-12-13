using UnityEngine;

// Should separate into Character and Enemy status appliers
public class CharacterStatusApplier : MonoBehaviour
{
    [SerializeField] private StatusEffectData[] sed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Character unit = other.gameObject.GetComponent<Character>();
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
