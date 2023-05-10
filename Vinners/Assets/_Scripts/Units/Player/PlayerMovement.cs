using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // replace this with UnitStats later
    [SerializeField]
    private float _speed = 3f;
    
    private Vector2 _moveInput;
   // private Vector2 _facingPos;
    private Rigidbody2D _rigidBody;

    private void Awake()
    {
        
        _rigidBody = GetComponent<Rigidbody2D>();

    }

    public void movePlayer(PlayerActions playerActions)
    {
        _moveInput = playerActions.PlayerInput.Movement.ReadValue<Vector2>();
        _rigidBody.velocity = _moveInput * _speed;

        // maybe make this less bandaidy
        Vector3 mousePos = playerActions.PlayerInput.Aim.ReadValue<Vector2>();
        Vector3 targetDirection  = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90; //-90 coz its facing left for some reason...
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        
        //todo for anim: if the z rotation is <0 should faceright, elsee faceleft
    }
    


}
