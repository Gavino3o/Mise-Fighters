using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class ViewManager : MonoBehaviour
{
    [SerializeField] private View[] views;
    [SerializeField] bool autoInitialise;
    [SerializeField] private View defaultView;
    public static ViewManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (autoInitialise) Initialise();
    }

    public void Initialise()
    {
        foreach (View v in views)
        {
            v.Initialise();

            v.Hide();
        }

        if (defaultView != null) defaultView.Show();
    }

    public void Show<T>(object args = null) where T : View // T extends View
    {
        foreach (View v in views)
        {
            if (v is T)
            {
                v.Show(args);
            } else
            {
                v.Hide();
            }
        }
    }
}
