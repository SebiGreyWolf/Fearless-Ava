using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public int maxHits = 3;
    public float fadeDuration = 1f;
    public float fadeStep = 0.1f;

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

        float alphaValue = originalColor.a;

        for (float t = 0; t < fadeDuration; t += fadeStep)
        {
            alphaValue = Mathf.Lerp(originalColor.a, 0, t / fadeDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alphaValue);
            yield return new WaitForSeconds(fadeStep);
        }

        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        Destroy(gameObject);
    }
}
