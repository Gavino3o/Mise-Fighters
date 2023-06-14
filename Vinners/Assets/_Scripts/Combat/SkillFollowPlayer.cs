using FishNet.Object;
using FishNet.Object.Synchronizing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class SkillFollowPlayer : NetworkBehaviour
{
    public float xOffset;
    public float yOffset;
    public int direction;
    [SyncVar] public GameObject player;

    private void FixedUpdate()
    {
        transform.position = new Vector3(player.transform.position.x +xOffset, player.transform.position.y + yOffset, player.transform.position.z);
    }
}
