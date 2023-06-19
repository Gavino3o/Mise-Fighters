using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using UnityEngine.InputSystem;

public sealed class InputCharacter : MonoBehaviour
{
    private PlayerActions playerActions;

    public Vector2 velocity;
    public Vector2 mousePos;
    public Vector2 targetDirection;

    private void OnEnable()
    {
        playerActions = new PlayerActions();
        EnableActions();
    }

    private void OnDisable()
    {
        DisableActions();
    }

    public void EnableActions()
    {
        playerActions.PlayerInput.Enable();
    }

    public void DisableActions()
    {
        playerActions.PlayerInput.Disable();
    }

    public void OnMovement(InputValue value)
    {
        velocity = value.Get<Vector2>();

    }

    public void OnAim(InputValue value)
    {
        if (Camera.main == null) return;
        mousePos = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
        targetDirection = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;
    }

}
