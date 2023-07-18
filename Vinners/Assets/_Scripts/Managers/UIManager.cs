
using UnityEngine;
using FishNet.Object;
using FishNet;

public class UIManager : NetworkBehaviour
{
    public static UIManager LocalInstance { get; private set; }

    [SerializeField] private View[] views;


    private void Start()
    {

    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        if (LocalInstance == null) LocalInstance = this;
        else Destroy(this);

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
