using UnityEngine;
using FishNet.Object;

public sealed class MoveCharacter : NetworkBehaviour
{
    private Character character;
    private Rigidbody2D rigidBody;
    private InputCharacter input;

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();
        input = GetComponent<InputCharacter>();
        rigidBody = GetComponent<Rigidbody2D>();
        character = GetComponent<Character>();
    }

    // handle movement, spellcasting which in turn handle the animations?
    private void Update()
    {
        if (!IsOwner) return;

        // Handle Movement
        rigidBody.velocity = input.moveInput * character.currMoveSpeed;
        
        // Handle Direction Faced
        Vector3 targetDirection = input.mousePos - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        /* TODO: if the z rotation is <0 should faceright, elsee faceleft
         * Keeping track of direction faced is good.
         * Should also keep track of mousePosition
         */
    }


}


