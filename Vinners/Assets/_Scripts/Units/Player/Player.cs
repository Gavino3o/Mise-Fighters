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
 */

public class Player : NetworkBehaviour
{
    public static Player LocalInstance { get; private set; }
    
    [SyncVar] public string username = "unset";
    [SyncVar] public bool isLockedIn;
    [SyncVar] public Character controlledCharacter;
    
    [SerializeField] private GameObject characterPrefab;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!base.IsOwner)
            return;

        LocalInstance = this;

        UIManager.LocalInstance.Initialise();
        UIManager.LocalInstance.Show<CharacterSelect>();
       
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        GameManager.Instance.players.Add(this);
        GameManager.Instance.playerCount++;
    }

    /*
     * Called when player disconnects from server.
     */
    public override void OnStopServer()
    {
        base.OnStopServer();
        GameManager.Instance.players.Remove(this);
        GameManager.Instance.playerCount--;
    }

    private void FixedUpdate()
    {
        if (!base.IsOwner) return;
    }

    /*
     * Assigns the player's chosen Character.
     */
    [ServerRpc] 
    public void ServerChooseCharacter(GameObject character)
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

    [ServerRpc]
    public void ServerSetUsername(string s)
    {
        if (s != null) username = s;
    }

    /*
     * Upon game start, spawns the Player Object and handles the UI changes
     */
    public void SpawnCharacter()
    {
        GameObject instance = Instantiate(characterPrefab);
        Spawn(instance, Owner);
        controlledCharacter = instance.GetComponent<Character>();
        controlledCharacter.controllingPlayer = this;
        TargetCharacterSpawned(Owner);
    }

    [ServerRpc]
    public void ServerRespawnCharacter()
    {
        TargetCharacterSpawned(Owner);
    }

    public void RespawnCharacter()
    {
        
        controlledCharacter.Revive();
    }

    public void CharacterDeath()
    {
        TargetCharacterDied(Owner);
    }

    /*
     * Shows the UI for GameInfo, has to be chain invoked (?) by the GameManager.
     */
    [TargetRpc]
    private void TargetCharacterSpawned(NetworkConnection conn)
    {
        UIManager.LocalInstance.Show<GameInfo>();
        ServerSetLockIn(false);
    }

    [TargetRpc]
    private void TargetStageClear(NetworkConnection conn)
    {
        UIManager.LocalInstance.Show<ReadyScreen>();
    }

    [TargetRpc]
    private void TargetCharacterDied(NetworkConnection conn)
    {
        Debug.Log("You died bozo");
        UIManager.LocalInstance.Show<Respawn>();
    }
}
