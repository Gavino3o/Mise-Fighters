using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using UnityEngine.UI;

public class Character : NetworkBehaviour
{
    public Image characterSplash;
    [SerializeField] private UnitStats baseStats;

    [SyncVar] public Player controllingPlayer;

    [SyncVar] public float health;
    [SyncVar] public float attack;
    [SyncVar] public float attackFreqSeconds;
    [SyncVar] public float moveSpeed;

    public void takeDamage(float n)
    {
        float next = health -= n;
        if (next >= baseStats.maxHealth)
        {
            health = baseStats.maxHealth;
        } else
        {
            health = Mathf.Max(next, 0f);
        }
    }

    // TODO: Alterations to other stats and handling of status effects

}
