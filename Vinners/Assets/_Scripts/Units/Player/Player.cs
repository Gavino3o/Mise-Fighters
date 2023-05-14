using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Connection;

public class Player : NetworkBehaviour
{
    // player should be localInstance
    public static Player LocalInstance { get; private set; }
    
    [SyncVar] public string username;
    [SyncVar] public bool isLockedIn;
    
    // every player has a reference to their controller character and vice versa
    // [SyncVar] public Character controlledCharacter;
    
    //just temporary
    [SerializeField] private GameObject characterPrefab;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!base.IsOwner)
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

    public override void OnStopServer()
    {
        base.OnStopServer();
        GameManager.Instance.players.Remove(this);
    }

    private void FixedUpdate()
    {
        if (!base.IsOwner) return;
    }

    [ServerRpc]
    public void ChooseCharacter(GameObject character)
    {
        this.characterPrefab = character;
    }
    
    [ServerRpc(RequireOwnership =false)]
    public void ServerSetLockIn(bool value)
    {
        isLockedIn = value;
    }

    // this is attached to player for testing purposes. 
    // change to lobby later? where the buttons instead call the game manager's methods?
    public void StartGame()
    {
        GameObject instance = Instantiate(characterPrefab);
        Spawn(instance, Owner);
        TargetCharacterSpawned(Owner);
        
        // controlledCharacter = instance.GetComponent<Character>();
    }

    [TargetRpc]
    private void TargetCharacterSpawned(NetworkConnection conn)
    {
        UIManager.Instance.Show<GameInfo>();
    }
}
