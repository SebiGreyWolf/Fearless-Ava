using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    public Player player;
    public float targetingDistance = 15f;
    private float attackCooldown = 0.75f;
    private float cooldownTimer = 1f;
    private SpriteRenderer spriteRenderer;

    public GameObject projectile;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Flip();

        cooldownTimer += Time.deltaTime;

        if (PlayerInTargetingDistance())
        {

            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                newProjectile.SetActive(true);
            }
        }


        //Debug.DrawLine(transform.position, player.transform.position);
    }

    private void Flip()
    {
        if (player.transform.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
    }

    public bool PlayerInTargetingDistance()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= targetingDistance)
            return true;
        return false;
    }
}
