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
        if (movementScript == null)
        {
            Setup();
        }
    }

    private void Setup()
    {
        movementScript = gameObject.GetComponent<AIPath>();
        movementScript.orientation = OrientationMode.YAxisForward;
        movementScript.gravity = new Vector3(0, 0, 0);
        movementScript.enableRotation = false;
        movementScript.canMove = true;
        movementScript.isStopped = false;
    }

    [ObserversRpc]
    public void StartAstarMovement()
    {
        if (movementScript == null)
        {
            Setup();
        }
        movementScript.canMove = true;
        movementScript.isStopped = false;
    }

    [ObserversRpc]
    public void StopAstarMovement()
    {
        if (movementScript == null)
        {
            Setup();
        }
        movementScript.canMove = false;
        movementScript.isStopped = true;
    }

    public void SetMaxMovementSpeed(float speed)
    {
        if (!IsServer) { return; }

        if (movementScript == null)
        {
            Setup();
        }

        movementScript.maxSpeed = speed;
    }

    public bool IsActive()
    {
        return movementScript.canMove && movementScript.isStopped;
    }

}
