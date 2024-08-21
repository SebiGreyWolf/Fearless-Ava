using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour, IDataPersistance
{
    public HealthBar healthBar;
    public Transform respawnPoint;

    public int maxHealth = 100;
    public float invulnerabilityDuration = 0.5f;

    public bool isFullyShieldBlock;
    public float reducedDamageBlock;

    private Rigidbody2D rb;
    public int currentHealth;
    private bool isInvulnerable = false;

    //SaveData
    public string currentLevel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBar.SetMaxHealth(maxHealth);
        currentHealth = maxHealth;
        currentLevel = SceneManager.GetActiveScene().ToString();
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

    public void LoadData(GameData data)
    {
        this.currentLevel = data.level;
        this.currentHealth = data.health;
        this.transform.position = new Vector3(data.position[0], data.position[1], data.position[2]);
    }

    public void SaveData(ref GameData data)
    {
        data.level = this.currentLevel;
        data.health = this.currentHealth;
        data.position[0] = this.transform.position.x;
        data.position[1] = this.transform.position.y;
        data.position[2] = this.transform.position.z;
    }
}