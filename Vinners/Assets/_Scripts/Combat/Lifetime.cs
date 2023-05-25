using UnityEngine;

public class Lifetime : MonoBehaviour
{
    public float lifetime = 1f;

    private void Update()
    {
        if (lifetime <= 0) Destroy(gameObject);

        lifetime -= Time.deltaTime;
    }
}
