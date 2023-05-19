using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Connection;

public abstract class EnemyProjectile : NetworkBehaviour
{
    public double _damage;
    public double _maxLifeTime;
    public float _speed;
}
