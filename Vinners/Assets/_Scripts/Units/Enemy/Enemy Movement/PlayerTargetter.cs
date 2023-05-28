using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pathfinding.AIDestinationSetter;

public class PlayerTargetter : MonoBehaviour
{
    /**
     * Targets a random active player at start.
     */
    private void Start()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        int rand = UnityEngine.Random.Range(0, players.Length);
        gameObject.GetComponent<Pathfinding.AIDestinationSetter>().target = players[rand].transform;
    }
}
