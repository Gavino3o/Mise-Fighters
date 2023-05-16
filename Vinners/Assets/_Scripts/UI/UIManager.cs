using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private View[] views;

    private void Awake()
    {
        Instance = this;
    }

    public void Initialise()
    {
        foreach (View v in views) v.Initialise();
    }

    public void Show<T>() where T : View
    {
        foreach (View v in views)
        {
            if (v.gameObject != null) v.gameObject.SetActive(v is T);
        }
    }

    
}
