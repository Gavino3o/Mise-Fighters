using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using UnityEngine.InputSystem;
using System;

public class CastCharacter : NetworkBehaviour
{
    protected InputCharacter input;
    protected Character character;
    protected Rigidbody2D rigidBody;
    protected MoveCharacter movement;

    public SpellData[] spellData = new SpellData[3];
    public readonly bool[] canCast = new bool[3];

    private void Awake()
    {
        character = GetComponent<Character>();
        input = character.input; 
        rigidBody = character.rb;
        movement = character.movement;

        Array.Fill(canCast, true);
    }

    // for now just a dummy class so that the Gameinfo can access it
    public IEnumerator Cooldown(int id)
    {
        canCast[id] = false;
        yield return new WaitForSeconds(spellData[id].cooldown);
        canCast[id] = true;
    }
}
