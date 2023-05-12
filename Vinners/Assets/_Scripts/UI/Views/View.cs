using UnityEngine;
/**
 * 
 */
public abstract class View : MonoBehaviour
{
    public bool isInitialised { get; private set; }

    public virtual void Initialise()
    {
        isInitialised = true;
    }


    // default argument is null, can be overridden to do pattern matching
    public virtual void Show(object args = null)
    {
        gameObject.SetActive(true);
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
}
