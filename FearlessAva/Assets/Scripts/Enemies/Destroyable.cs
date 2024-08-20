using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public int maxHealth = 15;
    public float fadeDuration = 1f;
    public float fadeStep = 0.1f;

    private int currentHealth;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Call this method when the object is hit
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        FindObjectOfType<AudioManagement>().PlaySound("EnemyHit");
        if (currentHealth <= 0)
        {
            Destroy();
        }
    }

    public void Destroy()
    {
        StartCoroutine(FadeOutAndDestroy());

        if (GetComponent<Pickup>())
            GetComponent<Pickup>().AutoPickup();
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
