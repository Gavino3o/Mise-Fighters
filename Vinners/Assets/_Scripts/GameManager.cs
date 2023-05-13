using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using System.Linq;
using FishNet.Object.Synchronizing;


/* 
 * Right now this class is just for managing lobbies, keeping track of players currently connected and in charge of starting the game
 * or changing scenes?
 */
public sealed class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    // list of players currently in the game
    [SyncObject] public readonly SyncList<Player> players = new();
    [SyncVar] public bool canStart;

    private void Awake()
    {
        Instance = this;

    }

    private void Update()
    {
        if (!IsServer) return;

        canStart = players.All(player => player.isReady);
        Debug.Log(canStart);
    }

    [Server]
    public void StartGame()
    {
        if (!canStart) return;
        // change scene maybe change scene global should bring all players with it.
        // then we just use those player objects to spawn their characters
        for (int i = 0; i < players.Count; i++)
        {
            players[i].StartGame();
        }
    }

    [Server]
    public void ChangeScene()
    {
        
    }
}
