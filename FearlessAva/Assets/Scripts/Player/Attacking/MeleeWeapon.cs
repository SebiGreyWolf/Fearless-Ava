using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    [SerializeField]
    private int damageAmount = 20;
    private PlayerMovement character;
    private Rigidbody2D rb;
    private MeleeAttackManager meleeAttackManager;
    private bool collided;
    private bool downwardStrike;

    private void Start()
    {
        character = GetComponentInParent<PlayerMovement>();
        rb = GetComponentInParent<Rigidbody2D>();
        meleeAttackManager = GetComponentInParent<MeleeAttackManager>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyHealth>())
        {
            HandleCollision(collision.GetComponent<EnemyHealth>());
        }
        if (collision.GetComponent<Destroyable>())
        {
            collision.GetComponent<Destroyable>().TakeDamage(damageAmount);
        }
    }

    private void HandleCollision(EnemyHealth objHealth)
    {
        if (Input.GetAxis("Vertical") < 0 && !character.IsGrounded)
        {
            downwardStrike = true;
            collided = true;
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
