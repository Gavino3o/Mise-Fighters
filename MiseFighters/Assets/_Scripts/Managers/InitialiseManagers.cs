using UnityEngine;
using FishNet;

public class InitialiseManagers : MonoBehaviour
{
    [SerializeField] private GameObject[] managerList;

    private void Start()
    {
        foreach (GameObject obj in managerList)
        {
            GameObject spawn = Instantiate(obj);
            InstanceFinder.ServerManager.Spawn(spawn);
        }
    }
}
