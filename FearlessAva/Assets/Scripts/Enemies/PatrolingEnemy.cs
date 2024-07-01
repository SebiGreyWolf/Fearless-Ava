using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PatrolingEnemy : MonoBehaviour
{
    public Player player;
    public GameObject playerPrefab;
    public EnemyPatrol enemyPatrol;
    public Destroyable destroyable;

    public float detectionRange = 1.75f;
    private float detectionAngle = 45.0f;

    private float attackCooldown = 2.0f;
    private int damage = 10;
    private float cooldownTimer = 1f;

    private void Awake()
    {
        if (playerPrefab != null)
        {
            player = playerPrefab.GetComponent<Player>();
        }

        destroyable = gameObject.GetComponent<Destroyable>();
    }

    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if(isPlayerInRange())
        {

            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                DamagePlayer();
            }
        }

        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !isPlayerInRange();
        }

    }

    void DamagePlayer()
    {
        player.TakeDamage(damage);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if (collision.relativeVelocity.y <= -0.5)
            {
                destroyable.Destroy();
            }
        }
    }

    public void ApplyIceEffect(float slowDuration)
    {
        if (enemyPatrol != null)
        {
            enemyPatrol.ApplySlow(slowDuration);
        }
    }

    bool isPlayerInRange() 
    {
        if (this != null && player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= detectionRange)
            {
                Vector3 directionToTarget = (player.transform.position - transform.position).normalized;
                Vector3 referenceForward = transform.right * transform.localScale.x;
                float angle = Vector3.Angle(referenceForward, directionToTarget);


                if (angle < detectionAngle)
                {
                    return true;
                }
            }
        }
        return false; 
    }
}
