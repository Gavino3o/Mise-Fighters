using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecrementCounterOnDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnDestroy()
    {
        EnemyManager.DecrementCounter();
    }
}
