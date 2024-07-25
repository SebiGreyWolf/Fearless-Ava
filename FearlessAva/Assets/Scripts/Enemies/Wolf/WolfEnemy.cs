using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WolfEnemy : MonoBehaviour
{
    public Player player;
    public float targetingDistance = 15f;
    private float attackCooldown = 1f;
    private float cooldownTimer = 1f;
    private SpriteRenderer[] spriteRenderer;
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
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        destroyable = gameObject.GetComponent<Destroyable>();
    }

    void Update()
    {
        //Flip();

        cooldownTimer += Time.deltaTime;

        if (PlayerInTargetingDistance())
        {

            if (cooldownTimer >= attackCooldown)
            {
                animator.StopPlayback();
                animator.Play("WurfAnimation");
                animator.SetBool("isThrowing", true);

                Debug.Log("Throwing");

                cooldownTimer = 0;
                GameObject newProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
                newProjectile.SetActive(true);
                FindObjectOfType<AudioManagement>().PlaySound("BoneThrow");
            }
        }

        DoT();
    }

    public void OnThrowingAnimFinished()
    {
        animator.SetBool("isThrowing", false);
        animator.Play("WolfIdle");
    }

    public void ApplyFireEffect(int amountOfFireDamageOverTime)
    {

        burnDamage = amountOfFireDamageOverTime;
        isBurning = true;

        foreach(SpriteRenderer renderer in spriteRenderer)
        {
            renderer.color = Color.red;
        }
    }

    private void DoT()
    {
        if (isBurning)
        {
            currentBurningDuration += Time.deltaTime;

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
                foreach (SpriteRenderer renderer in spriteRenderer)
                {
                    renderer.color = Color.white;
                }
            }
        }
    }
    /*
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
    */
    public bool PlayerInTargetingDistance()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= targetingDistance)
            return true;
        return false;
    }
}
