using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    // Common variables
    public float attackRange = 0.5f;
    public float cooldown = 1f;
    public Transform attackPoint;
    public TrailRenderer trailRenderer;
    public Animator animator;

    // Basic Attack variables
    public int basicAttackDamage = 5;
    public Sprite basicSword;

    // Ice Attack variables
    public int iceAttackDamage = 2;
    public float slowDuration = 2f;
    public Sprite iceSword;

    // Fire Attack variables
    public int fireAttackDamage = 2;
    public float fireDamage = 1f;
    public Sprite fireSword;

    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    private SpriteRenderer spriteRenderer;
    private Collider2D playerCollider;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();

        if (attackPoint == null)
            attackPoint = gameObject.GetComponentInChildren<Transform>();

        if (animator == null)
            animator = gameObject.GetComponent<Animator>();

        if (trailRenderer == null)
            trailRenderer = gameObject.GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && CanAttack())
        {
            PerformAttack(basicSword, basicAttackDamage, "isBasicAttack");
        }
        else if (Input.GetKeyDown(KeyCode.E) && CanAttack())
        {
            PerformAttack(iceSword, iceAttackDamage, "isIceAbility");
        }
        else if (Input.GetKeyDown(KeyCode.Q) && CanAttack())
        {
            PerformAttack(fireSword, fireAttackDamage, "isFireAbility");
        }
    }

    private void PerformAttack(Sprite swordSprite, int damage, string animationBool)
    {
        spriteRenderer.sprite = swordSprite;
        isAttacking = true;
        animator.SetBool(animationBool, true);
        trailRenderer.emitting = true;
    }

    public void PerformBasicAttack()
    {
        PerformAttackDamage(basicAttackDamage);
    }

    public void PerformIceAttack()
    {
        PerformAttackDamage(iceAttackDamage, true);
    }

    public void PerformFireAttack()
    {
        PerformAttackDamage(fireAttackDamage, false, true);
    }

    private void PerformAttackDamage(int damage, bool isIceAttack = false, bool isFireAttack = false)
    {
        Collider2D[] hitDestroyables = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D destroyable in hitDestroyables)
        {
            if (destroyable != playerCollider)
            {
                Destroyable damageable = destroyable.GetComponent<Destroyable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage);
                    if (isIceAttack)
                    {
                        ApplyIceEffect(damageable);
                    }
                    else if (isFireAttack)
                    {
                        ApplyFireEffect(damageable);
                    }
                }
            }
        }
    }

    public void FinishBasicAttack()
    {
        FinishAttack("isBasicAttack");
    }

    public void FinishIceAttack()
    {
        FinishAttack("isIceAbility");
    }

    public void FinishFireAttack()
    {
        FinishAttack("isFireAbility");
    }

    private void FinishAttack(string animationBool)
    {
        isAttacking = false;
        trailRenderer.emitting = false;
        animator.SetBool(animationBool, false);
        lastAttackTime = Time.time;
    }

    private bool CanAttack()
    {
        return (Time.time >= lastAttackTime + cooldown && !isAttacking);
    }

    private void ApplyIceEffect(Destroyable damageable)
    {
        PatrolingEnemy enemy = damageable.GetComponent<PatrolingEnemy>();
        if (enemy != null)
        {
            enemy.ApplyIceEffect(slowDuration);
        }
    }

    private void ApplyFireEffect(Destroyable damageable)
    {
        PatrolingEnemy enemy = damageable.GetComponent<PatrolingEnemy>();
        if (enemy != null)
        {
            enemy.ApplyFireEffect(fireDamage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        // Draw attack range gizmo
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
