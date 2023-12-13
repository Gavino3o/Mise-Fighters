using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * General script for locking rotation of an object while still following the transform of a target.
 */
public class LockRotation : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] Vector3 offset;

    private void Update()
    {
        transform.position = target.transform.position + offset;
        transform.rotation = Quaternion.identity;
    }
}
