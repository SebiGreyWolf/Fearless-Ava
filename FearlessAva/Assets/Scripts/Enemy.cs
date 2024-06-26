using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int damage = 10;
    public float moveSpeed = 2f;
    public Transform damageZone;
    public Vector2 damageZoneSize = new Vector2(2f, 2f);
    public float sightHight = 3f;
    public float coneAngle = 20f;
    public float coneDistance = 10f;
    public LayerMask playerLayer;

    public bool isSlowed = false;
    public float slowTimer = 0f;

    private bool isFacingRight = true;
    private Transform player;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player != null)
        {
            bool wasFacingRight = isFacingRight;

            if (IsPlayerInCone())
            {
                // If the player is within the cone, move towards the player
                if (player.position.x < transform.position.x)
                {
                    // Move left
                    transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
                    isFacingRight = false;
                }
                else
                {
                    // Move right
                    transform.Translate(Vector2.right * moveSpeed * Time.deltaTime);
                    isFacingRight = true;
                }
            }

            if (wasFacingRight != isFacingRight)
            {
                Flip();
            }

        }

        if (IsPlayerInDamageZone())
        {
            InflictDamageToPlayer();
        }

        if (isSlowed)
        {
            spriteRenderer.color = Color.blue;
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0f)
            {
                isSlowed = false;
                moveSpeed *= 2;
                spriteRenderer.color = Color.white;
            }
        }
    }

    private bool IsPlayerInCone()
    {
        Vector2 origin = transform.position;

        int layerMask = ~LayerMask.GetMask("Enemy");

        // Check right diagonal
        Vector2 rightDiagonal = Quaternion.Euler(0, 0, coneAngle) * Vector2.up;
        RaycastHit2D hitRight = Physics2D.Raycast(origin, rightDiagonal, coneDistance, layerMask);
        Debug.DrawRay(origin, rightDiagonal * coneDistance, Color.yellow);

        // Check left diagonal
        Vector2 leftDiagonal = Quaternion.Euler(0, 0, -coneAngle) * Vector2.up;
        RaycastHit2D hitLeft = Physics2D.Raycast(origin, leftDiagonal, coneDistance, layerMask);
        Debug.DrawRay(origin, leftDiagonal * coneDistance, Color.yellow);

        if ((hitRight.collider != null && hitRight.collider.CompareTag("Player")) ||
            (hitLeft.collider != null && hitLeft.collider.CompareTag("Player")))
        {
            return true;
        }

        return false;
    }

    private bool IsPlayerInDamageZone()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(damageZone.position, damageZoneSize, 0f);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    private void InflictDamageToPlayer()
    {
        Player playerHealth = player.GetComponent<Player>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
        }
    }

    public void ApplyIceEffect(float slowDuration)
    {
        if (!isSlowed)
        {
            isSlowed = true;
            slowTimer = slowDuration;
            moveSpeed /= 2;
        }
    }

    private void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    #region Debug
    private void OnDrawGizmosSelected()
    {
        // Draw the damage zone box
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(damageZone.position, damageZoneSize);
    }
    #endregion
}
