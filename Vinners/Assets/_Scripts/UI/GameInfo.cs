using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : View
{

    private void Update()
    {
        if (!Initialised) return;
    }

    public override void Initialise()
    {
        Debug.Log("UI View changed to Game Info");
        base.Initialise();
    }
}
