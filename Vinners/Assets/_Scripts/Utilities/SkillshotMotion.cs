using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillshotMotion : NetworkBehaviour
{
    [SerializeField] private float projectileSpeed;
    private Vector2 movementDirection = Vector2.zero;

    private void Update()
    {
        transform.position += (Vector3)movementDirection * Time.deltaTime * projectileSpeed;
    }

    public void SetDirection(Vector2 dir)
    {
        movementDirection = dir.normalized;
    }
}