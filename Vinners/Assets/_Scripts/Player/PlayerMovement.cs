using FishNet.Object;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : NetworkBehaviour
{
    // replace this with UnitStats later
    [SerializeField]
    private float _speed = 3f;
    private PlayerActions _playerActions;
    private Rigidbody2D _rigidBody;
    private Vector2 _moveInput;

    private void Awake()
    {
        _playerActions = new PlayerActions();
        _rigidBody = GetComponent<Rigidbody2D>();

    }

    private void OnEnable()
    {
        _playerActions.PlayerInput.Enable();
    }

    private void OnDisable()
    {
        _playerActions.PlayerInput.Disable();
    }

    private void FixedUpdate()
    {
        _moveInput = _playerActions.PlayerInput.Movement.ReadValue<Vector2>();
        _rigidBody.velocity = _moveInput * _speed;
    }
}
