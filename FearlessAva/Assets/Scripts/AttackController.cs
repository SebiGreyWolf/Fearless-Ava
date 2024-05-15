using UnityEngine;

public class SwordController : MonoBehaviour
{
    public Animator animator;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;

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
        //Trigger Animation
        animator.SetTrigger("Attack");

        // Set flag to indicate that an attack is in progress
        isAttacking = true;
    }

    // This method is called from the animation event
    public void PerformAttack()
    {
        // Detect enemies within the attack range
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        // Deal damage to each enemy
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit enemy for damage");
        }
    }

    // This method is called from the animation event to indicate the end of the attack animation
    public void FinishAttack()
    {
        // Reset flag to indicate that the attack has finished
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        // Draw attack range gizmo
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}