using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public sealed class CastCharacter : NetworkBehaviour
{
    private Rigidbody2D rigidBody;
    private InputCharacter input;



    // TODO: implement spell functionality
    // [SerializeField] private Spell skill;
    // [SerializeField] private Spell dash;
    // [SerializeField] private Spell ultimate;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        input = GetComponent<InputCharacter>();
        rigidBody = GetComponent<Rigidbody2D>();
    }
  
    private void Update()
    {
        if (!IsOwner) return;

        if (input.skillPressed)
        {
            CastSpell();
        }
    }

    [ServerRpc]
    public void CastSpell()
    {
        // lets the server know that this player has case a spell.
        Debug.Log($"{gameObject} belonging to {Owner} casted Skill");
    }

    [ObserversRpc]
    public void HandleSpellCast()
    {
        // after casting spell what to do
    }

}
