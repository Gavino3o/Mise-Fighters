using UnityEngine;
using FishNet.Object;

public sealed class MoveCharacter : NetworkBehaviour
{
    public Character character;
    private Rigidbody2D rigidBody;
    public InputCharacter input;

    public bool interrupted;

    public override void OnStartClient()
    {
        base.OnStartClient();
        character = GetComponent<Character>();
        input = character.input;
        rigidBody = character.rb;
    }

    // handle movement, spellcasting which in turn handle the animations?
    private void Update()
    {
        if (!IsOwner) return;
        if (Camera.main == null) return;
        if (interrupted) return;

        rigidBody.velocity = 5f * character.currMoveSpeed * input.velocity;
 
        float angle = Mathf.Atan2(input.targetDirection.y, input.targetDirection.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }
}