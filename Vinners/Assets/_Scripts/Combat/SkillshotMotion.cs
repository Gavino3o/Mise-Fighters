using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillshotMotion : NetworkBehaviour
{
    [SerializeField] private float projectileSpeed;
    public Vector2 movementDirection;

    private void FixedUpdate()
    {
        transform.position += projectileSpeed * Time.deltaTime * (Vector3)movementDirection;
    }

}