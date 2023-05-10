using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : NetworkBehaviour
{
    private PlayerActions _playerActions;
    private Rigidbody2D _rigidBody;
    private PlayerMovement _movementHandler;

    private void Awake()
    {
        _playerActions = new PlayerActions();
        _movementHandler = GetComponent<PlayerMovement>();
    }
    // Update is called once per frame
    private void OnEnable()
    {
        _playerActions.PlayerInput.Enable();
    }

    private void OnDisable()
    {
        _playerActions.PlayerInput.Disable();
    }

    // handle movement, spellcasting which in turn handle the animations?
    private void FixedUpdate()
    {
        if (!base.IsOwner) return;
        _movementHandler.movePlayer(_playerActions);
        
        // how to do the spell cast here otherwise its just checking every frame
    }
}
