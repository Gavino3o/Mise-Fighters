using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineUIManager : MonoBehaviour
{
    public static OfflineUIManager LocalInstance { get; private set; }

    [SerializeField] private View[] views;

    private void Awake()
    {
        LocalInstance = this;
    }

    private void Start()
    {
        Initialise();
        Show<MainMenu>();
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
