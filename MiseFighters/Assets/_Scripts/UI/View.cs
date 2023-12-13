using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class View : MonoBehaviour
{
    public bool Initialised { get; private set; }

    public virtual void Initialise()
    {
        Initialised = true;
    }

}
