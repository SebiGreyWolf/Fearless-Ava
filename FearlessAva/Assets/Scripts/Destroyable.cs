using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public int maxHits = 3;

    private int currentHits;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        currentHits = 0;
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }

    // Call this method when the object is hit
    public void TakeHit()
    {
        currentHits++;

        if (currentHits >= maxHits)
        {
            StartCoroutine(FadeOutAndDestroy());
        }
    }

    private IEnumerator FadeOutAndDestroy()
    {
        // Disable the collider to allow the player to pass through
        boxCollider.enabled = false;

        Color originalColor = spriteRenderer.color;
        float fadeDuration = 1f; // Duration of the fade out
        float fadeStep = 0.1f; // Time step for each fade iteration
        float alphaValue = originalColor.a;

        for (float t = 0; t < fadeDuration; t += fadeStep)
        {
            alphaValue = Mathf.Lerp(originalColor.a, 0, t / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alphaValue);
            yield return new WaitForSeconds(fadeStep);
        }

        // Ensure the object is completely invisible and destroy it
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        Destroy(gameObject);
    }

    // Optional: Visual feedback for each hit (e.g., changing color or playing an animation)
    private IEnumerator FlashSprite()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }
}
