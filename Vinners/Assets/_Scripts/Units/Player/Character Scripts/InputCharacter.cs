using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;


public sealed class InputCharacter : NetworkBehaviour
{
    [SerializeField] private float funFactor = 5f; 
    
    private PlayerActions _playerActions;

    // all information required by external input handling classes
    public Vector2 moveInput;
    public Vector3 mousePos;
    public bool skillPressed;

    // reading local playerinput and updating this information inside itself
    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        _playerActions = new PlayerActions();
        _playerActions.PlayerInput.Enable();

        
    }

    public override void OnStopNetwork()
    {
        base.OnStopNetwork();
        _playerActions.PlayerInput.Disable();
    }

    private void Update()
    {
        if (!IsOwner) return;

        moveInput = _playerActions.PlayerInput.Movement.ReadValue<Vector2>() * funFactor;
        mousePos = Camera.main.ScreenToWorldPoint(_playerActions.PlayerInput.Aim.ReadValue<Vector2>());
        skillPressed = _playerActions.PlayerInput.Skill.triggered;
    }
}
