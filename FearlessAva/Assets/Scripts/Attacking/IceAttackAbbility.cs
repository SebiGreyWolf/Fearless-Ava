using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAttackAbbility : MonoBehaviour
{
    public int attackDamage = 2;
    public float attackRange = 0.5f;
    public float cooldown = 1f;
    public Transform attackPoint;
    public TrailRenderer trailRenderer;
    public Animator animator;
    public float slowDuration = 2f;

    private bool isAttacking = false;
    private float lastAttackTime = 0f;

    private void Start()
    {

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
            DoIceAttack();
        }
    }

    private void DoIceAttack()
    {
        isAttacking = true;
        animator.SetBool("isIceAbbility", true);
        trailRenderer.emitting = true;
    }

    public void PerformIceAttack()
    {
        Collider2D[] hitDestroyables = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D destroyable in hitDestroyables)
        {
            Destroyable damageable = destroyable.GetComponent<Destroyable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
                ApplyIceEffect(damageable);
            }
        }
    }

    public void FinishIceAttack()
    {
        isAttacking = false;
        trailRenderer.emitting = false;
        animator.SetBool("isIceAbbility", false);
        lastAttackTime = Time.time;
    }

    private bool CanAttack()
    {
        return (Time.time >= lastAttackTime + cooldown && !isAttacking);
    }
    
    private void ApplyIceEffect(Destroyable damageable)
    {
        Enemy enemy = damageable.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.ApplyIceEffect(slowDuration);
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
