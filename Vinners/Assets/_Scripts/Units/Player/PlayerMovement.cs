using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

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

        // Handle Movement
        _moveInput = _playerActions.PlayerInput.Movement.ReadValue<Vector2>();
        _rigidBody.velocity = _moveInput * _speed;

        // maybe make this less bandaidy
        Vector3 mousePos = _playerActions.PlayerInput.Aim.ReadValue<Vector2>();
        Vector3 targetDirection = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90; //-90 coz its facing left for some reason...
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));

        //todo for anim: if the z rotation is <0 should faceright, elsee faceleft

        // how to do the spell cast here otherwise its just checking every frame
    }


}


