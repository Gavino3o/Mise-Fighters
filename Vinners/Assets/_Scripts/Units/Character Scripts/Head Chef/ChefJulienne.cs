using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChefJulienne : MonoBehaviour
{
    private BoxCollider2D _collider;
    public float interval = 0.25f;


    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    private void Start()
    {
        StartCoroutine(ToggleCollider(interval));
    }

    private IEnumerator ToggleCollider(float interval)
    {
        while (true)
        {
            _collider.enabled = true;
            yield return new WaitForSeconds(interval);
            _collider.enabled = false;
            yield return new WaitForSeconds(interval);
        }
    }
}
