using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReworkedMeeleWeapon : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 20;
    private ReworkedPlayerMovement character;
    private Rigidbody2D rb;
    private ReworkedMeleeAttackManager meleeAttackManager;
    private bool collided;
    private bool downwardStrike;

    private void Start()
    {
        character = GetComponentInParent<ReworkedPlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();
        meleeAttackManager = GetComponentInParent<ReworkedMeleeAttackManager>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<ReworkedEnemyHeath>())
        {
            HandleCollision(collision.GetComponent<ReworkedEnemyHeath>());
        }
    }

    private void HandleCollision(ReworkedEnemyHeath objHealth)
    {
        if (Input.GetAxis("Vertical") < 0 && !character.IsGrounded)
        {
            downwardStrike = true;
            collided = true;
            character.ResetJumpAndDash();
        }

        objHealth.Damage(damageAmount);
        StartCoroutine(NoLongerColliding());
    }

    private void HandleMovement()
    {
        if (collided && downwardStrike)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0); // Reset current vertical velocity
            rb.AddForce(Vector2.up * meleeAttackManager.upwardsForce, ForceMode2D.Impulse);
            collided = false;
            downwardStrike = false;
        }
    }

    private IEnumerator NoLongerColliding()
    {
        yield return new WaitForSeconds(meleeAttackManager.movementTime);
        collided = false;
        downwardStrike = false;
    }
}
