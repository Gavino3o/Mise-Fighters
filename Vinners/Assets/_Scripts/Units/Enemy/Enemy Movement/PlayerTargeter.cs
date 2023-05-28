using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pathfinding.AIDestinationSetter;

public class PlayerTargeter : MonoBehaviour
{
    private AIDestinationSetter destinationSetter;
    private GameObject targetPlayer;

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        destinationSetter = gameObject.GetComponent<AIDestinationSetter>();
        var players = GameObject.FindGameObjectsWithTag("Player");
        int rand = UnityEngine.Random.Range(0, players.Length);
        targetPlayer = players[rand];
        destinationSetter.target = targetPlayer.transform;
    }
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
        return targetPlayer;
    }
}
