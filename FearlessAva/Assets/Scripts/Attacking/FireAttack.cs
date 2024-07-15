using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAttack : MonoBehaviour
{

    public int attackDamage = 2;
    public float attackRange = 0.5f;
    public float cooldown = 1f;
    public Transform attackPoint;
    public TrailRenderer trailRenderer;
    public Animator animator;
    public Sprite fireSword;

    private bool isAttacking = false;
    private float lastAttackTime = 0f;
    private float fireDamage = 1f;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (attackPoint == null)
            attackPoint = this.GetComponentInChildren<Transform>();

        if (animator == null)
            animator = gameObject.GetComponent<Animator>();

        if (trailRenderer == null)
            trailRenderer = gameObject.GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanAttack())
        {
            spriteRenderer.sprite = fireSword;
            DoFireAttack();
        }
    }

    private void DoFireAttack()
    {
        isAttacking = true;
        animator.SetBool("isFireAbbility", true);
        trailRenderer.emitting = true;
    }

    public void PerformFireAttack()
    {
        Collider2D[] hitDestroyables = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D destroyable in hitDestroyables)
        {
            Destroyable damageable = destroyable.GetComponent<Destroyable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
                ApplyFireEffect(damageable);
            }
        }
    }

    public void FinishFireAttack()
    {
        isAttacking = false;
        trailRenderer.emitting = false;
        animator.SetBool("isFireAbbility", false);
        lastAttackTime = Time.time;
    }

    private bool CanAttack()
    {
        return (Time.time >= lastAttackTime + cooldown && !isAttacking);
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
