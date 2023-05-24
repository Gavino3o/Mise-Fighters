using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/StatusEffectData")]
public class StatusEffectData : ScriptableObject
{
    public string effectName;
    public float durationSeconds = 0;
    public float damageOverTime = 0;
    public float attackMultiplier = 1;
    public float moveSpeedMultiplier = 1;
    // public VisualEffect visualEffect;

}
