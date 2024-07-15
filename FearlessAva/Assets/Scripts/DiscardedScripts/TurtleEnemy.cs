using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleEnemy : MonoBehaviour
{
    private float attackCooldown = 1.0f;
    private int damage = 10;
    private float cooldownTimer = Mathf.Infinity;

    public float range = 0.5f;
    public float colliderDistance = 2.5f;

    public BoxCollider2D boxCollider;
    public LayerMask playerLayer;
    public GameObject playerPrefab;
    public Player player;

    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        playerLayer = LayerMask.GetMask("Player");

        playerPrefab = GameObject.FindGameObjectWithTag("Player");
        enemyPatrol = GetComponentInParent<EnemyPatrol>();

        if(playerPrefab != null )
        {
            player = playerPrefab.GetComponent<Player>();
        }
    }


    void Update()
    {
        cooldownTimer = Time.deltaTime;

        Vector2 direction = player.transform.position - transform.position;
        float distance = direction.magnitude;
        direction.Normalize();

        Debug.DrawRay(transform.position, direction * distance);
        Debug.Log("Player Pos: " + player.transform.position);
        Debug.Log("Turtle Pos: " + transform.position);


        if (PlayerInSight())
        {
            Debug.Log("Player in Sight");

            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
            }
        }

        if(enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }

        
    }


    private bool PlayerInSight()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z), 
            0, Vector2.left, 0, playerLayer);

        


        if(raycastHit.collider != null) 
        {
            DamagePlayer();
        }


        return raycastHit.collider != null;
    }

    private void DamagePlayer()
    {
        if (PlayerInSight())
        {
            player.TakeDamage(damage);
        }
    }




    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }


}
