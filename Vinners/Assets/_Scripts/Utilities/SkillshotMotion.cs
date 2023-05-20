using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillshotMotion : NetworkBehaviour
{
    [SerializeField] private float projectileSpeed;
    [SyncVar] private Vector2 movementDirection = Vector2.zero;

    private void Update()
    {
        GetComponent<Rigidbody2D>().velocity = movementDirection * projectileSpeed;
    }

    public void SetDirection(Vector2 dir)
    {
        movementDirection = dir.normalized;
    }
}
