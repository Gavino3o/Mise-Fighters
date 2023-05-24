using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusApplier : MonoBehaviour
{
    [SerializeField] private StatusEffectData sed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Unit unit = other.gameObject.GetComponent<Character>();
        if (unit != null)
        {
            unit.ApplyStatusEffect(sed);
            Debug.Log($"Status effect {sed.effectName} applied to {other.gameObject}!");
        }
    }

}
