using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public HealthBar healthBar;
    public Transform respawnPoint;

    public int maxHealth = 16;
    public float invulnerabilityDuration = 0.5f;

    public bool isFullyShieldBlock;
    public float reducedDamageBlock;

    private Rigidbody2D rb;
    private int currentHealth;
    private bool isInvulnerable = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (!isInvulnerable && !isFullyShieldBlock)
        {
            if (reducedDamageBlock > 0)
            {
                amount = (int)(amount * reducedDamageBlock);
                Debug.Log(amount);
            }
            currentHealth -= amount;
            healthBar.SetHealth(currentHealth);
            if (currentHealth <= 0)
            {
                Debug.Log("YOU ARE FUCKING DEAD! (LOSER)");
                gameObject.transform.position = respawnPoint.transform.position;
                healthBar.SetHealth(maxHealth);
                currentHealth = maxHealth;
            }
            StartCoroutine(InvulnerabilityTimer());
        }
    }

    private IEnumerator InvulnerabilityTimer()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityDuration);
        isInvulnerable = false;
    }
}