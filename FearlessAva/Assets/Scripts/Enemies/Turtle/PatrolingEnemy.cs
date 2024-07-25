using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PatrolingEnemy : MonoBehaviour
{
    public Player player;
    public GameObject playerPrefab;
    public EnemyPatrol enemyPatrol;
    public Destroyable destroyable;
    private SpriteRenderer[] spriteRenderer;

    private float detectionRange = 4f;
    public float detectionAngle = 45.0f;

    [Header("Attacking")]
    private float attackCooldown = 2.0f;
    public int damage = 10;
    private float cooldownTimer = 1f;

    [Header("Fire Damage")]
    private bool isBurning = false;
    private float burnDuration = 3f;
    private float secondsAlreadyBurning = 0f;
    private float currentBurningDuration = 0f;
    private int burnDamage = 0;


    private void Awake()
    {
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();

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

        DoT();
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

    public void ApplyFireEffect(int amountOfFireDamageOverTime)
    {
        if (enemyPatrol != null)
        {
            burnDamage = amountOfFireDamageOverTime;
            isBurning = true;
            foreach (SpriteRenderer renderer in spriteRenderer)
            {
                renderer.color = Color.red;
            }
        }
    }

    private void DoT()
    {
        if(isBurning)
        {
            currentBurningDuration += Time.deltaTime;
            //Debug.Log(currentBurningDuration);
            if(currentBurningDuration >= 1 && secondsAlreadyBurning < burnDuration)
            {
                Debug.Log("BURN");
                secondsAlreadyBurning++;
                destroyable.TakeDamage(burnDamage);
                currentBurningDuration = 0;
            }
            else if(burnDuration == secondsAlreadyBurning)
            {
                Debug.Log("Burn Over");
                currentBurningDuration = 0;
                secondsAlreadyBurning = 0;
                isBurning = false;
                foreach (SpriteRenderer renderer in spriteRenderer)
                {
                    renderer.color = Color.white;
                }
            }
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

                //Debug.DrawLine(this.transform.position, player.transform.position);
                Debug.Log(angle);


                if (angle > 180-detectionAngle)
                {
                    return true;
                }
            }
        }
        return false; 
    }
}
