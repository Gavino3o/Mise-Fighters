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
        FindNewTargetPlayer();
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

    public void ChangeTargetDuration(GameObject obj, float duration)
    {
        StartCoroutine(Taunted(obj, duration));
    }

    private IEnumerator Taunted(GameObject obj, float duration)
    {
        GameObject tmp = targetPlayer;
        ChangeTargetPlayer(obj);
        yield return new WaitForSeconds(duration);
        ChangeTargetPlayer(tmp);
    }

    public GameObject GetCurrentTargetPlayer()
    {
        if (destinationSetter == null)
        {
            Setup();
        }

        if (targetPlayer == null)
        {
           FindNewTargetPlayer();
        }

        return targetPlayer;
    }

    public GameObject FindNewTargetPlayer()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        int rand = UnityEngine.Random.Range(0, players.Length);
        targetPlayer = players[rand];

        return targetPlayer;
    }
}
