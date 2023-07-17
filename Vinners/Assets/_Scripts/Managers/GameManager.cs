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
 * or changing scenes.
 */
public sealed class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    // List of players currently in the game
    [SyncObject] public readonly SyncList<Player> players = new();
 
    [SyncVar] public bool canStart;

    [SyncVar] public int playerCount;

    [Header("Scene Names")]
    public string[] sceneNames;

    private int currentScene;
    
    /*public string startScene;
    public string stageOne;
    public string stageTwo;
    public string stageThree;
    */
    
    private void Awake()
    {
            Instance = this;
    }

    /*
     * Track if all Players are locked in.
     */
    private void Update()
    {
        if (!IsServer) return;

        canStart = players.All(player => player.isLockedIn);
        
    }

    /*
     * Starts the Game for all Players.
     */
    [Server]
    public void StartGame()
    {
        if (!canStart) return;
        // change scene maybe change scene global should bring all players with it.
        // then we just use those player objects to spawn their characters

        // Both this is hardcoded. Change in the future
        ChangeScene(0);
        Vector3 spawnPoint = new(34, 20, 0);

        int rand = Random.Range(2, 5);
        AudioManager.Instance.ObserversPlayBackgroundMusic(rand, true);

        for (int i = 0; i < players.Count; i++)
        {
            players[i].SpawnCharacter(spawnPoint);
        }
    }

    /*
     * Changes the scene and brings all owned objects with it
     */
    [Server]
    public void ChangeScene(int stage)
    {
        currentScene = stage;
        if (sceneNames[stage] == null)
        {
            Victory();
        }

        SceneLoadData sld = new(sceneNames[stage]);
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

    [Server]
    public void StageClear()
    {
        for (int i = 0; i < players.Count; i++)
        {
            players[i].StageCleared();
        }
    }

    [Server]
    public void LoadNextScene()
    {
        currentScene++;
        ChangeScene(currentScene);

        for (int i = 0; i < players.Count; i++)
        {
            players[i].EnterNextScene();
        }
    }

    [Server]
    public void Victory()
    {
        // win!
    }

}
