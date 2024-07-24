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
    public Destroyable destroyable;

    public GameObject projectile;

    [Header("Fire Damage")]
    private bool isBurning = false;
    private float burnDuration = 3f;
    private float secondsAlreadyBurning = 0f;
    private float currentBurningDuration = 0f;
    private int burnDamage = 0;

    [Header("Animation")]
    public Animator animator;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Flip();

        cooldownTimer += Time.deltaTime;

        if (PlayerInTargetingDistance())
        {

            if (cooldownTimer >= attackCooldown)
            {
                animator.SetBool("isThrowing", true);
                cooldownTimer = 0;
                GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                newProjectile.SetActive(true);
                FindObjectOfType<AudioManagement>().PlaySound("BoneThrow");
            }
            else
            {
                animator.SetBool("isThrowing", false);
            }
        }   
        //Debug.DrawLine(transform.position, player.transform.position);
    }

    public void ApplyFireEffect(int amountOfFireDamageOverTime)
    {

        burnDamage = amountOfFireDamageOverTime;
        isBurning = true;
        spriteRenderer.color = Color.red;

    }

    private void DoT()
    {
        if (isBurning)
        {
            currentBurningDuration += Time.deltaTime;
            //Debug.Log(currentBurningDuration);
            if (currentBurningDuration >= 1 && secondsAlreadyBurning < burnDuration)
            {
                Debug.Log("BURN");
                secondsAlreadyBurning++;
                destroyable.TakeDamage(burnDamage);
                currentBurningDuration = 0;
            }
            else if (burnDuration == secondsAlreadyBurning)
            {
                Debug.Log("Burn Over");
                currentBurningDuration = 0;
                secondsAlreadyBurning = 0;
                isBurning = false;
                spriteRenderer.color = Color.white;
            }
        }
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
