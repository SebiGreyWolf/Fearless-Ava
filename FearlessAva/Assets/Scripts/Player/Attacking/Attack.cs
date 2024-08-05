using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    //Basic Attack
    public int basicAttackDamage = 5;
    public float basicAttackRange = 0.5f;
    public float basicAttackCooldown = 1f;
    public Sprite basicSword;

    //Ice Attack
    public int iceAttackDamage = 2;
    public float iceAttackRange = 0.5f;
    public float iceAttackCooldown = 3f;
    public float slowDuration = 2f;
    public Sprite iceSword;
    public ParticleSystem iceSwordParticle;
    public ParticleSystem iceSwordGlow;

    //Fire Attack
    public int fireAttackDamage = 2;
    public float fireAttackRange = 0.5f;
    public float fireAttackCooldown = 1f;
    public int fireDamage = 1;
    public Sprite fireSword;
    public ParticleSystem fireSwordParticle;
    public ParticleSystem fireSwordGlow;

    //Shield
    public float shieldDuration = 5f;
    public float fullBlockDuration = 1f;
    public float damageReduction = 0.5f; // 50% damage reduction after full block
    public float shieldCooldown = 10f;

    //Common variables
    public Transform attackPoint;
    public TrailRenderer trailRenderer;
    public Animator animator;
    public GameObject shieldObject;
    public ParticleSystem hitParticles;

    private bool isAttacking = false;
    public bool isShielded = false;
    private float lastBasicAttackTime = 0f;
    public float lastIceAttackTime = 0f;
    public float lastFireAttackTime = 0f;
    public float lastShieldUseTime = 0f;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer shieldRenderer;
    private Collider2D playerCollider;
    private Material originalMaterial;
    private Player player;
    private PlayerMovement playerMovement;
    private Rigidbody2D rb;
    private Vector2 direction;
    private bool collided;
    private bool downwardStrike;

    [SerializeField]
    private float upwardsForce = 10f;
    [SerializeField]
    private float defaultForce = 5f;
    [SerializeField]
    private float movementTime = 0.1f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        originalMaterial = spriteRenderer.material;
        player = GetComponentInParent<Player>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        rb = GetComponent<Rigidbody2D>();

        if (attackPoint == null)
            attackPoint = gameObject.GetComponentInChildren<Transform>();

        if (animator == null)
            animator = gameObject.GetComponent<Animator>();

        if (trailRenderer == null)
            trailRenderer = gameObject.GetComponent<TrailRenderer>();

        if (shieldObject != null)
        {
            shieldRenderer = shieldObject.GetComponentInChildren<SpriteRenderer>();
            shieldObject.SetActive(false); // Ensure the shield is initially inactive
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && CanAttack(lastBasicAttackTime, basicAttackCooldown))
        {
            spriteRenderer.sprite = basicSword;
            DoBasicAttack();
        }
        else if (Input.GetKeyDown(KeyCode.E) && CanAttack(lastIceAttackTime, iceAttackCooldown))
        {
            spriteRenderer.sprite = iceSword;
            PlayAbilityParticles(iceSwordParticle, iceSwordGlow);
            DoIceAttack();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && CanAttack(lastFireAttackTime, fireAttackCooldown))
        {
            spriteRenderer.sprite = fireSword;
            PlayAbilityParticles(fireSwordParticle, fireSwordGlow);
            DoFireAttack();
        }
        else if (Input.GetKeyDown(KeyCode.W) && CanUseShield(lastShieldUseTime, shieldCooldown))
        {
            StartCoroutine(ActivateShield());
        }
    }

    private void DoBasicAttack()
    {
        isAttacking = true;
        animator.SetBool("isBasicAttack", true);
        trailRenderer.emitting = true;
        AudioManagement g = FindObjectOfType<AudioManagement>();
        g.PlaySound("Attack");
    }

    public void PerformBasicAttack()
    {
        Collider2D[] hitDestroyables = Physics2D.OverlapCircleAll(attackPoint.position, basicAttackRange);
        foreach (Collider2D destroyable in hitDestroyables)
        {
            if (destroyable != playerCollider)
            {
                Destroyable damageable = destroyable.GetComponent<Destroyable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(basicAttackDamage);
                    PlayHitParticles(damageable.gameObject);
                    if (damageable.name.Contains("baumstumpf"))
                    {
                        FindObjectOfType<AudioManagement>().PlaySound("TreeBreak");
                    }
                    HandleCollision(damageable.GetComponent<EnemyHealth>());
                }
            }
        }
    }

    public void FinishBasicAttack()
    {
        isAttacking = false;
        trailRenderer.emitting = false;
        animator.SetBool("isBasicAttack", false);
        lastBasicAttackTime = Time.time;
    }

    private void DoIceAttack()
    {
        isAttacking = true;
        animator.SetBool("isIceAbility", true);
        trailRenderer.emitting = true;
        FindObjectOfType<AudioManagement>().PlaySound("Attack");
    }

    public void PerformIceAttack()
    {
        Collider2D[] hitDestroyables = Physics2D.OverlapCircleAll(attackPoint.position, iceAttackRange);
        foreach (Collider2D destroyable in hitDestroyables)
        {
            Destroyable damageable = destroyable.GetComponent<Destroyable>();
            if (damageable != null)
            {
                damageable.TakeDamage(iceAttackDamage);
                ApplyIceEffect(damageable);
                PlayHitParticles(damageable.gameObject);
                HandleCollision(damageable.GetComponent<EnemyHealth>());
            }
        }
    }

    public void FinishIceAttack()
    {
        StopAbilityParticles(iceSwordParticle, iceSwordGlow);
        isAttacking = false;
        trailRenderer.emitting = false;
        animator.SetBool("isIceAbility", false);
        lastIceAttackTime = Time.time;
    }

    private void DoFireAttack()
    {
        isAttacking = true;
        animator.SetBool("isFireAbility", true);
        trailRenderer.emitting = true;
        FindObjectOfType<AudioManagement>().PlaySound("Attack");
    }

    public void PerformFireAttack()
    {
        Collider2D[] hitDestroyables = Physics2D.OverlapCircleAll(attackPoint.position, fireAttackRange);
        foreach (Collider2D destroyable in hitDestroyables)
        {
            Destroyable damageable = destroyable.GetComponent<Destroyable>();
            if (damageable != null)
            {
                damageable.TakeDamage(fireAttackDamage);
                ApplyFireEffect(damageable);
                PlayHitParticles(damageable.gameObject);
                HandleCollision(damageable.GetComponent<EnemyHealth>());
            }
        }
    }

    public void FinishFireAttack()
    {
        StopAbilityParticles(fireSwordParticle, fireSwordGlow);
        isAttacking = false;
        trailRenderer.emitting = false;
        animator.SetBool("isFireAbility", false);
        lastFireAttackTime = Time.time;
    }

    public bool CanAttack(float lastAttackTime, float cooldown)
    {
        return (Time.time >= lastAttackTime + cooldown && !isAttacking);
    }

    public bool CanUseShield(float lastShieldTime, float cooldown)
    {
        return (Time.time >= lastShieldTime + cooldown && !isShielded);
    }

    private void ApplyIceEffect(Destroyable damageable)
    {
        PatrolingEnemy turtle = damageable.GetComponent<PatrolingEnemy>();
        if (turtle != null)
        {
            turtle.ApplyIceEffect(slowDuration);
        }
        WolfEnemy wolf = damageable.GetComponent<WolfEnemy>();
        if (wolf != null)
        {
            //wolf.ApplyIceEffect(slowDuration);
        }
    }

    private void ApplyFireEffect(Destroyable damageable)
    {
        PatrolingEnemy turtle = damageable.GetComponent<PatrolingEnemy>();
        if (turtle != null)
        {
            turtle.ApplyFireEffect(fireDamage);
        }
        WolfEnemy wolf = damageable.GetComponent<WolfEnemy>();
        if (wolf != null)
        {
            wolf.ApplyFireEffect(fireDamage);
        }
    }

    private IEnumerator ActivateShield()
    {
        isShielded = true;
        lastShieldUseTime = Time.time;
        float shieldEndTime = Time.time + shieldDuration;
        float fullBlockEndTime = Time.time + fullBlockDuration;
        FindObjectOfType<AudioManagement>().PlaySound("ShieldActive");

        shieldObject.SetActive(true);
        SetShieldAlpha(1f);

        while (Time.time < shieldEndTime)
        {
            if (Time.time < fullBlockEndTime)
            {
                player.isFullyShieldBlock = true;
            }
            else
            {
                player.isFullyShieldBlock = false;
                player.reducedDamageBlock = damageReduction;
                SetShieldAlpha(0.7f); // Reduce alpha after full block
                FindObjectOfType<AudioManagement>().PlaySound("ShieldWeakening");
            }

            yield return null;
        }
        player.reducedDamageBlock = 0;
        isShielded = false;
        shieldObject.SetActive(false);
        FindObjectOfType<AudioManagement>().PlaySound("ShieldDeactive");
    }

    private void SetShieldAlpha(float alpha)
    {
        if (shieldRenderer != null)
        {
            Color color = shieldRenderer.color;
            color.a = alpha;
            shieldRenderer.color = color;
        }
    }

    private void PlayAbilityParticles(ParticleSystem ps, ParticleSystem glow)
    {
        ps.Play();
        glow.Play();
    }

    private void PlayHitParticles(GameObject hitEnemy)
    {
        hitParticles.transform.position = hitEnemy.transform.position;
        hitParticles.Play();
    }

    private void StopAbilityParticles(ParticleSystem ps, ParticleSystem glow)
    {
        ps.Play();
        glow.Play();
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, basicAttackRange);
        Gizmos.DrawWireSphere(attackPoint.position, iceAttackRange);
        Gizmos.DrawWireSphere(attackPoint.position, fireAttackRange);
    }

    private void FixedUpdate()
    {
        //Uses the Rigidbody2D AddForce method to move the player in the correct direction
        HandleMovement();
    }

    private void HandleCollision(EnemyHealth objHealth)
    {
        if (objHealth == null) return;

        //Checks to see if the GameObject allows for upward force and if the strike is downward as well as grounded
        if (objHealth.giveUpwardForce && Input.GetAxis("Vertical") < 0 && !playerMovement.IsGrounded)
        {
            //Sets the direction variable to up
            direction = Vector2.up;
            //Sets downwardStrike to true
            downwardStrike = true;
            //Sets collided to true
            collided = true;
        }
        if (Input.GetAxis("Vertical") > 0 && !playerMovement.IsGrounded)
        {
            //Sets the direction variable to down
            direction = Vector2.down;
            //Sets collided to true
            collided = true;
        }
        //Checks to see if the melee attack is a standard melee attack
        if ((Input.GetAxis("Vertical") <= 0 && playerMovement.IsGrounded) || Input.GetAxis("Vertical") == 0)
        {
            //Checks to see if the player is facing left; if you don't have a character script, the commented out line of code can also check for that
            if (!playerMovement.IsFacingRight) //(transform.parent.localScale.x < 0)
            {
                //Sets the direction variable to right
                direction = Vector2.right;
            }
            else
            {
                //Sets the direction variable to left
                direction = Vector2.left;
            }
            //Sets collided to true
            collided = true;
        }
        //Deals damage in the amount of damageAmount
        objHealth.Damage(basicAttackDamage); // Assuming basicAttackDamage is the damageAmount
        //Coroutine that turns off all the bools related to melee attack collision and direction
        StartCoroutine(NoLongerColliding());
    }

    private void HandleMovement()
    {
        //Checks to see if the GameObject should allow the player to move when melee attack collides
        if (collided)
        {
            //If the attack was in a downward direction
            if (downwardStrike)
            {
                //Propels the player upwards by the amount of upwardsForce in the meleeAttackManager script
                rb.AddForce(direction * upwardsForce);
            }
            else
            {
                //Propels the player backwards by the amount of defaultForce in the meleeAttackManager script
                rb.AddForce(direction * defaultForce);
            }
        }
    }

    //Coroutine that turns off all the bools that allow movement from the HandleMovement method
    private IEnumerator NoLongerColliding()
    {
        //Waits in the amount of time setup by the meleeAttackManager script; this is by default .1 seconds
        yield return new WaitForSeconds(movementTime);
        //Turns off the collided bool
        collided = false;
        //Turns off the downwardStrike bool
        downwardStrike = false;
    }
}
