using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using UnityEngine.InputSystem;

public sealed class InputCharacter : MonoBehaviour
{

    private PlayerActions _playerActions;

    public Vector2 velocity;
    public Vector2 mousePos;

    // reading local playerinput and updating this information inside itself
    private void OnEnable()
    {
        _playerActions = new PlayerActions();
        _playerActions.PlayerInput.Enable();
    }

    private void OnDisable()
    {
        _playerActions.PlayerInput.Disable();
    }
    public void OnMovement(InputValue value)
    {
        velocity = value.Get<Vector2>();

    }

    public void OnAim(InputValue value)
    {
        if (Camera.main == null) return;
        mousePos = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
    }

}