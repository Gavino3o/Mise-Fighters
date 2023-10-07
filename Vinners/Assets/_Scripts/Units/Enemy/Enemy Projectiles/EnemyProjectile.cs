using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;

public abstract class EnemyProjectile : NetworkBehaviour
{
    public float damage;
    public float maxLifeTime;
    public float speed;
    public Vector3 targetPosition;
}
