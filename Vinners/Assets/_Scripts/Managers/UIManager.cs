using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

public class UIManager : NetworkBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private View[] views;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void Initialise()
    {
        foreach (View v in views) v.Initialise();
    }

    public void Show<T>() where T : View
    {
        foreach (View v in views)
        {
            if (v != null) v.gameObject.SetActive(v is T);
        }
    }
}
