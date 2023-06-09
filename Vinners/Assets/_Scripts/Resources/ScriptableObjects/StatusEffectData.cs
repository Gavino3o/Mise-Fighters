using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/StatusEffectData")]
/*
 * Store data about status effects. Units will use this data to apply effects on themselves.
 */
public class StatusEffectData : ScriptableObject
{
    public static float IGNITION_DMG = 7f;
    
    public string effectName;
    public float durationSeconds = 0;
    public float damageOverTime = 0;
    public float attackMultiplier = 1;
    public float moveSpeedMultiplier = 1;
    public bool douser;
    public bool igniter;
}
