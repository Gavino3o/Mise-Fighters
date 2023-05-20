using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public sealed class CastCharacter : NetworkBehaviour
{
    private Character character; 
    private Rigidbody2D rigidBody;
    private InputCharacter input;

    // TODO: implement spell functionality
    [SerializeField] private Spell skill;
    // [SerializeField] private Spell dash;
    // [SerializeField] private Spell ultimate;

    /*
     * [SerializeField] private Spell[] spellList;
     */

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        input = GetComponent<InputCharacter>();
        rigidBody = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        // have to spawn a dummy spellcaster object
        skill = Instantiate(skill, transform);
        ServerManager.Spawn(skill.gameObject, Owner);

        /*
        * foreach (Spell spell in spellList) {
        *   spell = Instantiate(spell, transform);
        *   Spawn(spell.gameObject, Owner);
        * }
        */
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (input.skillPressed)
        {
            CastSkill(input.mousePos);
        }
    }

    [ServerRpc]
    // maybe should pass the spell the character instead
    public void CastSkill(Vector2 mousePos)
    {
        skill.Cast(mousePos);
    }

    [ObserversRpc]
    public void HandleSpellCast()
    {
        // after casting spell what to do
    }

}
