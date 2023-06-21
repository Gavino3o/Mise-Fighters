using UnityEngine;
using FishNet.Object;
using FishNet;

public sealed class MoveCharacter : NetworkBehaviour
{
    public Character character;
    private Rigidbody2D rigidBody;
    public InputCharacter input;

    public SpriteRenderer sprite;

    public bool interrupted;

    public override void OnStartClient()
    {
        base.OnStartClient();
        character = GetComponent<Character>();
        input = character.input;
        rigidBody = character.rb;
        sprite = character.sprite;
    }

    // handle movement, spellcasting which in turn handle the animations?
    private void FixedUpdate()
    {
        if (!IsOwner) return;
        if (Camera.main == null) return;
        if (interrupted) return;

        rigidBody.velocity = 5f * character.currMoveSpeed * input.velocity;
 
        // float angle = Mathf.Atan2(input.targetDirection.y, input.targetDirection.x) * Mathf.Rad2Deg - 90;
        // transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        if (input.targetDirection.x > 0)
        {
            SpriteFlip(false);
        } else if (input.targetDirection.x < 0) {
            SpriteFlip(true);
        }
    }

    [Client]
    private void SpriteFlip(bool value)
    {
        if (!IsOwner) return;
        sprite.flipX = value;
        ServerFlipSprite(value);
    }

    [ObserversRpc(BufferLast = true, ExcludeOwner = true)]
    private void ObserverSpriteFlip(bool value)
    {
        if (IsOwner) return;
        sprite.flipX = value;
    }

    [ServerRpc]
    private void ServerFlipSprite(bool value)
    {
        sprite.flipX = value;
        ObserverSpriteFlip(value);
    }
}