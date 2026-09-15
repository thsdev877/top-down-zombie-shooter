using System.Collections.Generic;
using UnityEngine;

public class RandomDecal : MonoBehaviour
{
    // This list stores all the possible decals that can show up
    public List<Sprite> decals;
    // SpriteRenderer
    SpriteRenderer spriteRenderer;

    void Start()
    {
        // Get the sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Random.Range(0, decals.Count - 1) means a random number from 0 to number of decals - 1
        // decals[..] means take that decal from the list
        // spriteRenderer.sprite means that we are setting the sprite of the sprite renderer to the random decal
        spriteRenderer.sprite = decals[Random.Range(0, decals.Count - 1)];
    }
}
