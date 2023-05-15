using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class AttackCharacter : NetworkBehaviour
{
    private Character character;
    private Rigidbody2D rigidBody;
    private InputCharacter input;

    private float lastAttacked;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        input = GetComponent<InputCharacter>();
        rigidBody = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
    }

    private void Update()
    {
        if (!IsOwner) return;

        if (Time.time - lastAttacked < character.attackFreqSeconds) return;
        lastAttacked = Time.time;
        AutoAttack();
        
    }

    [ServerRpc]
    public void AutoAttack()
    {
        Debug.Log($"{gameObject} controlled by {Owner} attacks!");
    }
}
