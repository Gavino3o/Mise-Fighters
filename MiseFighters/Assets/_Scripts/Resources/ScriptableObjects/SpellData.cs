using UnityEngine;

[CreateAssetMenu(menuName = "GameData/SpellData")]
public class SpellData : ScriptableObject
{
    public string spellName;
    public string description;
    public float cooldown;
    public float damage;
    public float duration;
}
