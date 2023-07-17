using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pathfinding.AIDestinationSetter;
using FishNet.Object;

public class PlayerTargeter : NetworkBehaviour
{
    private AIDestinationSetter destinationSetter;
    private GameObject targetPlayer;

    public void Start()
    {
        Setup();
    }

    public void Setup()
    {
        destinationSetter = gameObject.GetComponent<AIDestinationSetter>();
        var players = GameObject.FindGameObjectsWithTag("Player");
        int rand = UnityEngine.Random.Range(0, players.Length);
        targetPlayer = players[rand];
        destinationSetter.target = targetPlayer.transform;
    }

    [ObserversRpc]
    public void ChangeTargetPlayer(GameObject player)
    {
        if (destinationSetter == null)
        {
            Setup();
        }
        destinationSetter.target = player.transform;
    }

    public GameObject GetCurrentTargetPlayer()
    {
        if (destinationSetter == null)
        {
            Setup();
        }
        return targetPlayer;
    }
}
