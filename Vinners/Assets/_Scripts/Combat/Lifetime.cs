using System.Collections;
using UnityEngine;
using FishNet.Object;

public class Lifetime : NetworkBehaviour
{
    public float lifetime;

    public override void OnStartServer()
    {
        base.OnStartServer();
        StartCoroutine(Lifespan());
    }

    public IEnumerator Lifespan()
    {
        yield return new WaitForSeconds(lifetime);
        Despawn(gameObject);
    }
}
