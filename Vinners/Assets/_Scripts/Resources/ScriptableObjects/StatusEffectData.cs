using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/StatusEffectData")]
public class StatusEffectData : ScriptableObject
{
    public float durationSeconds;
    public float damageOverTime;
    public float attackDecrease;
    public float movementDecrease;
    // public VisualEffect visualEffect;

}
