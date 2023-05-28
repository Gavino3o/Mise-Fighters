using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using Pathfinding;
using System.Diagnostics.Tracing;
using Unity.VisualScripting;

public class EnemyMovementController : NetworkBehaviour
{
    protected AIPath movementScript;

    void Start()
    {
        Setup();
    }

    private void Setup()
    {
        if (movementScript == null)
        {
            movementScript = gameObject.GetComponent<AIPath>();
        }
        movementScript.orientation = OrientationMode.YAxisForward;
        movementScript.gravity = new Vector3(0, 0, 0);
        movementScript.enableRotation = false;
    }

    protected void StartAstarMovement()
    {
        Setup();
        movementScript.canMove = true;
        movementScript.isStopped = false;
    }

    protected void StopAstarMovement()
    {
        if (movementScript == null)
        {
            movementScript = gameObject.GetComponent<AIPath>();
        }
        movementScript.canMove = false;
        movementScript.isStopped = true;
    }

    protected void SetMaxMovementSpeed(float speed)
    {
        if (movementScript == null)
        {
            movementScript = gameObject.GetComponent<AIPath>();
        }

        movementScript.maxSpeed = speed;
    }

}
