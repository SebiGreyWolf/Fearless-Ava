using UnityEngine;

public class SwordController : MonoBehaviour
{
    public Animator animator;
    public ParticleSystem swordTrail;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public int attackDamage = 5;
    public LayerMask enemyLayer;
    public LayerMask destroyableLayer;

    private bool isAttacking = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking)
        {
            Attack();
        }
    }

    void Attack()
    {
        // Set flag to indicate that an attack is in progress
        isAttacking = true;
        swordTrail.Play();
        animator.SetBool("isAttacking", isAttacking);
    }

    // This method is called from the animation event
    public void PerformAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        Collider2D[] hitDestroyables = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, destroyableLayer);

        swordTrail.Stop();

        foreach (Collider2D enemy in hitEnemies)
        {
            Enemy e = enemy.GetComponent<Enemy>();
            if (e != null)
            {
                e.TakeDamage(attackDamage);
            }
        }

        foreach (Collider2D destroyable in hitDestroyables)
        {
            Destroyable destroyableObject = destroyable.GetComponent<Destroyable>();
            if (destroyableObject != null)
            {
                destroyableObject.TakeHit();
            }
        }
    }

    public void FinishAttack()
    {
        // Reset flag to indicate that the attack has finished
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        // Draw attack range gizmo
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
