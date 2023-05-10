using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/UnitStats")]
public class UnitStats : ScriptableObject
{
    public float hitPoints;
    public float attack;
    public float attackFreqSeconds;
    public float moveSpeed;
}
