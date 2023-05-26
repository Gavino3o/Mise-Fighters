using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet;

public class InitialiseManagers : MonoBehaviour
{
    [SerializeField] GameObject[] managerList;

    private void Start()
    {
        foreach (GameObject obj in managerList)
        {
            GameObject spawn = Instantiate(obj);
            InstanceFinder.ServerManager.Spawn(spawn);
        }
    }
}
