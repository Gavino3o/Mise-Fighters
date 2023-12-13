using UnityEngine;
using FishNet.Object;


public class CharacterDamager : NetworkBehaviour
{
    public float damage;
    // private Character chr = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Character chr = other.gameObject.GetComponent<Character>();
        DealDamage(chr);
    }

    
    [ServerRpc(RequireOwnership = false)]
    private void DealDamage(Character chr)
    {
        if (chr != null) chr.TakeDamage(damage);
    }
    

}
