using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSprite : MonoBehaviour
{
    public Sprite[] sprites;

    private void Start()
    {
        SpriteRenderer spr = GetComponent<SpriteRenderer>();
        if (spr != null) spr.sprite = sprites[Random.Range(0, sprites.Length)];
    }
}
