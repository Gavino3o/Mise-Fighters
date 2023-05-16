using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Connection;

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

    public override void OnStartNetwork()
    {
        base.OnStartNetwork();

        if (!base.Owner.IsLocalClient)
            return;

        LocalInstance = this;

        UIManager.Instance.Initialise();
        UIManager.Instance.Show<CharacterSelect>();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        GameManager.Instance.players.Add(this);
    }

    /*
     * Called when player disconnects from server.
     */
    public override void OnStopServer()
    {
        base.OnStopServer();
        GameManager.Instance.players.Remove(this);
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
        TargetCharacterSpawned(Owner);
    }

    /*
     * Shows the UI for GameInfo, has to be chain invoked (?) by the GameManager.
     */
    [TargetRpc]
    private void TargetCharacterSpawned(NetworkConnection conn)
    {   
        UIManager.Instance.Show<GameInfo>();
        
    }
}
