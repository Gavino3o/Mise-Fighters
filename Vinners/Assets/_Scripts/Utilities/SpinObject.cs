using System;
using UnityEngine;

// General script for making an object perpetually spin randomly
public class SpinObject : MonoBehaviour
{
    public float spinSpeed;
    private static System.Random rnd = new System.Random();
    private int funFactor;

    private void Start()
    {
        funFactor = rnd.Next(-5, 5);
    }

    private void Update()
    {
        transform.Rotate(0f, 0f, spinSpeed * 6 * funFactor * Time.deltaTime, Space.Self);
    }
}
