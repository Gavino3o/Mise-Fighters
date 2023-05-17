using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Managing.Scened;
using FishNet.Connection;
using System;

/*
 * The Player class is responsible for information relating to a player's account (their username, connection status
 * to host, controlled character etc.)
 * 
 * TODO: remove the many dependencies present
 */

public class Player : NetworkBehaviour
{
    // Local Instance of Player (per Owner)
    public static Player LocalInstance { get; private set; }
    
    [SyncVar] public string username = "unset";
    [SyncVar] public bool isLockedIn;
    
    // Every player has a reference to their controlled character and vice versa
    [SyncVar] public Character controlledCharacter;
    
    // just temporary until Characters get implemented
    [SerializeField] private GameObject characterPrefab;


    // TODO: Redistribute responsibility? or is this fine...coz this is pretty much the only network object going between scenes
    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!base.IsOwner)
            return;

        LocalInstance = this;

        UIManager.Instance.Initialise();
        UIManager.Instance.Show<CharacterSelect>();
       
    }

    // TERRIBLE BUT WORKS FOR NOW, surely i will fix this later 
    // somehow causes network manager to no longer recognise that i am host 
    // details: characterselect ui initialises with the new network manager that gets deleted instantly, so ishost is defaulted false
    public void ReinitialiseUI()
    {
        UIManager.Instance.Initialise();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        LobbyManager.Instance.players.Add(this);
        LobbyManager.Instance.playerCount++;
    }

    /*
     * Called when player disconnects from server.
     */
    public override void OnStopServer()
    {
        base.OnStopServer();
        LobbyManager.Instance.players.Remove(this);
        LobbyManager.Instance.playerCount--;
    }

    private void FixedUpdate()
    {
        if (!base.IsOwner) return;
    }

    /*
     * Assigns the player's chosen Character.
     */
    [ServerRpc] 
    public void ChooseCharacter(GameObject character)
    {
        characterPrefab = character;
    }
    
    /*
     * Informs the server that this player has locked in. This function is called from the Ready Button.
     */
    [ServerRpc(RequireOwnership = false)]
    public void ServerSetLockIn(bool value)
    {
        isLockedIn = value;
    }

    // TODO: instead of this one serverrpc call, we should use a usename manager so that all usernames are synced across all clients.
    // currently players can only see their own username and client only knows its own username. Not that important right now.
    [ServerRpc(RequireOwnership = true)]
    public void SetUsername(string s)
    {
        if (s != null)
        {
            username = s;
        }
    }
    /*
     * Upon game start, spawns the Player Object and handles the UI changes
     */
    public void SpawnCharacter()
    {
        GameObject instance = Instantiate(characterPrefab);
        Spawn(instance, Owner);
        controlledCharacter = instance.GetComponent<Character>();
        TargetCharacterSpawned(Owner);
    }

    /*
     * Shows the UI for GameInfo, has to be chain invoked (?) by the GameManager.
     */
    [TargetRpc]
    private void TargetCharacterSpawned(NetworkConnection conn)
    {
        //do nothing    
    }
}
