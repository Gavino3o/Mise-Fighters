using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using FishNet.Object.Synchronizing;

public class Player : NetworkBehaviour
{
    [SyncVar] public string username;
    [SyncVar] public bool isReady;
    [SerializeField] private GameObject canvasObject;
    // every player has a reference to their controller character and vice versa
    // [SyncVar] public Character controlledCharacter;
    
    //just temporary
    [SerializeField] private GameObject characterPrefab;

    public override void OnStartClient()
    {
        base.OnStartClient();

        if (!base.IsOwner)
            canvasObject.SetActive(false);
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
    public void IsReadyServer(bool ready)
    {
        isReady = ready;
    }

    // Want to have a ready system where can only start if host && if everyone is ready
    public void ToggleReady()
    {
        IsReadyServer(!isReady);
    }

    
    // this is attached to player for testing purposes. 
    // change to lobby later? where the buttons instead call the game manager's methods?
    public void StartGame()
    {
        GameObject instance = Instantiate(characterPrefab);
        Spawn(instance, Owner);
        canvasObject.SetActive(false);
        // controlledCharacter = instance.GetComponent<Character>();
    }

}
