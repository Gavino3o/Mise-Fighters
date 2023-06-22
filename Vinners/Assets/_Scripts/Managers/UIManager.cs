
using UnityEngine;
using FishNet.Object;

public class UIManager : NetworkBehaviour
{
    public static UIManager LocalInstance { get; private set; }

    [SerializeField] private View[] views;

    private void Awake()
    {
        LocalInstance = this;  
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
