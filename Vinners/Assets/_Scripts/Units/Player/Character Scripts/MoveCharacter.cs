using UnityEngine;
using FishNet.Object;
using UnityEngine.InputSystem;

public sealed class MoveCharacter : NetworkBehaviour
{
    private Character character;
    private Rigidbody2D rigidBody;
    private InputCharacter input;

    public override void OnStartClient()
    {
        base.OnStartClient();
        character = GetComponent<Character>();
        input = character.inputCharacter;
        rigidBody = character.rb;
    }

    // handle movement, spellcasting which in turn handle the animations?
    private void Update()
    {
        if (!IsOwner) return;
        if (Camera.main == null) return;

        rigidBody.velocity = 5f * character.currMoveSpeed * input.velocity;

        Vector2 targetDirection = input.mousePos - new Vector2(transform.position.x, transform.position.y);
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }
}