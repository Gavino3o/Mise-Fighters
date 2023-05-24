using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using UnityEngine.InputSystem;

public sealed class CastCharacter : NetworkBehaviour
{
    private Character character; 
    private Rigidbody2D rigidBody;
    private InputCharacter input;

    // TODO: implement spell functionality
    // [SerializeField] private Spell dash;
    // [SerializeField] private Spell ultimate;

    /*
     * [SerializeField] private Spell[] spellList;
     */

    public override void OnStartClient()
    {
        base.OnStartClient();
        // have to spawn a dummy spellcaster object

        input = GetComponent<InputCharacter>();
        rigidBody = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();

        /*
        * foreach (Spell spell in spellList) {
        *   spell = Instantiate(spell, transform);
        *   Spawn(spell.gameObject, Owner);
        * }
        */
    }

    public void OnSkill()
    {
        if (!IsOwner) return;
        CastSkill(input.mousePos);
    }
    

    [ServerRpc]
    // maybe should pass the spell the character instead
    public void CastSkill(Vector2 mousePos)
    {
    }

    [ObserversRpc]
    public void HandleSpellCast()
    {
        // after casting spell what to do
    }
}
