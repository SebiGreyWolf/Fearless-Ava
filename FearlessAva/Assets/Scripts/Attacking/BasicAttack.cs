using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicAttack : MonoBehaviour
{
    public int attackDamage = 5;
    public float attackRange = 0.5f;
    public float cooldown = 1f;
    public Transform attackPoint;
    public TrailRenderer trailRenderer;
    public Animator animator;
    public Sprite basicSword;

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
            spriteRenderer.sprite = basicSword;
            DoBasicAttack();
        }
    }

    private void DoBasicAttack()
    {
        isAttacking = true;
        animator.SetBool("isBasicAttack", true);
        trailRenderer.emitting = true;
    }

    public void PerformBasicAttack()
    {
        Collider2D[] hitDestroyables = Physics2D.OverlapCircleAll(attackPoint.position, attackRange);
        foreach (Collider2D destroyable in hitDestroyables)
        {
            if (destroyable != playerCollider)
            {
                Debug.Log("Hit: " + destroyable.name);
                Destroyable damageable = destroyable.GetComponent<Destroyable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(attackDamage);
                }
            }
        }
    }

    public void FinishBasicAttack()
    {
        isAttacking = false;
        trailRenderer.emitting = false;
        animator.SetBool("isBasicAttack", false);
        lastAttackTime = Time.time;
    }

    private bool CanAttack()
    {
        return (Time.time >= lastAttackTime + cooldown && !isAttacking);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        // Draw attack range gizmo
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
