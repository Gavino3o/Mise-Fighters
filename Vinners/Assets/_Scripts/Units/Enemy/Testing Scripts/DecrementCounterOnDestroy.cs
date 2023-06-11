using FishNet.Object;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecrementCounterOnDestroy : NetworkBehaviour
{
    // Start is called before the first frame update
    private void OnDestroy()
    {
        if (!IsServer) return;
        EnemyManager.DecrementCounter();
    }
}
