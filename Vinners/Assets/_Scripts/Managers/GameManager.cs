using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;
using FishNet.Object;
using System.Linq;
using FishNet.Object.Synchronizing;
using FishNet.Managing.Scened;
using FishNet.Connection;


/* 
 * Right now this class is just for managing lobbies, keeping track of players currently connected and in charge of starting the game
 * or changing scenes?
 */
public sealed class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    // List of players currently in the game
    [SyncObject] public readonly SyncList<Player> players = new();
 
    [SyncVar] public bool canStart;

    // just for debugging purposes
    [SyncVar] public int playerCount;
    
    private void Awake()
    {
            Instance = this;
    }

    /*
     * Track if all Players are locked in.
     * TODO: Likely just for current build, might have more update functions that could be acstracted out in the future.
     */
    private void Update()
    {
        if (!IsServer) return;

        canStart = players.All(player => player.isLockedIn);
        
    }

    /*
     * Starts the Game for all Players.
     * TODO: Likely just for current build, have to do proper state/scene change logic in the future.
     */
    [Server]
    public void StartGame()
    {
        if (!canStart) return;
        // change scene maybe change scene global should bring all players with it.
        // then we just use those player objects to spawn their characters

        ChangeScene("AlvinScene");

        for (int i = 0; i < players.Count; i++)
        {
            players[i].SpawnCharacter();
        }
    }

    [Server]
    public void ChangeScene(string sceneName)
    {
        SceneLoadData sld = new(sceneName);
        List<NetworkObject> movedObjects = new();
        foreach (NetworkConnection item in InstanceFinder.ServerManager.Clients.Values)
        {
            foreach (NetworkObject nob in item.Objects)
                movedObjects.Add(nob);
        }

        sld.MovedNetworkObjects = movedObjects.ToArray();
        sld.ReplaceScenes = ReplaceOption.All;
        InstanceFinder.SceneManager.LoadGlobalScenes(sld);
    }

}
