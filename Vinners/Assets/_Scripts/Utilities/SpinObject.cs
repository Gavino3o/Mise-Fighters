using System;
using UnityEngine;

// General script for making an object perpetually spin
public class SpinObject : MonoBehaviour
{
    public float spinSpeed;
    private static System.Random rnd = new System.Random();

    private void Update()
    {
        transform.Rotate(0f, 0f, spinSpeed * 6 * rnd.Next(-5, 5) * Time.deltaTime, Space.Self);
    }
}
