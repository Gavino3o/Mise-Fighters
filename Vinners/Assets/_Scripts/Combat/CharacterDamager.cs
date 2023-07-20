using UnityEngine;
using FishNet.Object;


public class CharacterDamager : NetworkBehaviour
{
    public float damage;
    private Character chr = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        chr = other.gameObject.GetComponent<Character>();
        if (chr != null) DealDamage();
    }

    [ServerRpc(RequireOwnership = false)]
    private void DealDamage()
    {
        if (chr == null) return;
        chr.TakeDamage(damage);
    }

}
