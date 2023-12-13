using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameData/UnitStats")]
public class UnitStats : ScriptableObject
{
    public float maxHealth;
    public float attack;
    public float attackFreqSeconds;
    public float moveSpeed;
}
