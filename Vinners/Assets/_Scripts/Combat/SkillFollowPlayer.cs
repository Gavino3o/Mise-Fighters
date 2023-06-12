using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SkillFollowPlayer : NetworkBehaviour
{
    private Vector2[] cardinalDirections = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    [SerializeField] private float offset;
    public int direction;
    public GameObject player;

    private void FixedUpdate()
    {
        transform.position = player.transform.position * cardinalDirections[direction] * offset;
    }
}
