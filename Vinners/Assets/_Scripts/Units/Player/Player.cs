using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Managing.Scened;
using FishNet.Connection;
using System;

/*
 * The Player class is responsible for information relating to a player (connection status
 * to host, controlled character, lives left etc.)
 */
public class Player : NetworkBehaviour
{
    public static Player LocalInstance { get; private set; }
    
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

    public override void OnStopServer()
    {
        base.OnStopServer();
        GameManager.Instance.players.Remove(this);
        GameManager.Instance.playerCount--;
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
     * Informs the server that this player has locked in.
     */
    [ServerRpc(RequireOwnership = false)]
    public void ServerSetLockIn(bool value)
    {
        isLockedIn = value;
    }

    /*
     * Upon game start, spawns the Character Object and handles the UI changes
     */
    public void SpawnCharacter(Vector3 spawnLocation)
    {
        GameObject instance = Instantiate(characterPrefab, spawnLocation, Quaternion.identity);
        Spawn(instance, Owner);
        controlledCharacter = instance.GetComponent<Character>();
        controlledCharacter.controllingPlayer = this;
        TargetCharacterSpawned(Owner);
    }

    public void StageCleared()
    {
        controlledCharacter.isInvicible = true;
        TargetStageClear(Owner);
    }

    public void EnterNextScene(Vector3 spawnPoint)
    {
        // reset position to middle of stage or set some spawnpoints
        controlledCharacter.isInvicible = false;
        controlledCharacter.transform.position = spawnPoint;

        RespawnCharacter();
    }

    public void RespawnCharacter()
    {
        if (GameManager.Instance.livesTotal <= 0) return;
        controlledCharacter.Revive();
    }

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


}
