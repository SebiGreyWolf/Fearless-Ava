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
    public float iceAttackCooldown = 1f;
    public float slowDuration = 2f;
    public Sprite iceSword;

    //Fire Attack
    public int fireAttackDamage = 2;
    public float fireAttackRange = 0.5f;
    public float fireAttackCooldown = 1f;
    public int fireDamage = 1;
    public Sprite fireSword;

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

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        originalMaterial = spriteRenderer.material;
        player = GetComponentInParent<Player>();

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
            DoIceAttack();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && CanAttack(lastFireAttackTime, fireAttackCooldown))
        {
            spriteRenderer.sprite = fireSword;
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
    }

    public void PerformBasicAttack()
    {
        Collider2D[] hitDestroyables = Physics2D.OverlapCircleAll(attackPoint.position, basicAttackRange);
        foreach (Collider2D destroyable in hitDestroyables)
        {
            if (destroyable != playerCollider)
            {
                Debug.Log("Hit: " + destroyable.name);
                Destroyable damageable = destroyable.GetComponent<Destroyable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(basicAttackDamage);
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
            }
        }
    }

    public void FinishIceAttack()
    {
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
            }
        }
    }

    public void FinishFireAttack()
    {
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
        PatrolingEnemy enemy = damageable.GetComponent<PatrolingEnemy>();
        if (enemy != null)
        {
            enemy.ApplyIceEffect(slowDuration);
        }
    }

    private void ApplyFireEffect(Destroyable damageable)
    {
        PatrolingEnemy enemy = damageable.GetComponent<PatrolingEnemy>();
        if (enemy != null)
        {
            enemy.ApplyFireEffect(fireDamage);
        }
    }

    private IEnumerator ActivateShield()
    {
        isShielded = true;
        lastShieldUseTime = Time.time;
        float shieldEndTime = Time.time + shieldDuration;
        float fullBlockEndTime = Time.time + fullBlockDuration;

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
            }

            yield return null;
        }
        player.reducedDamageBlock = 0;
        isShielded = false;
        shieldObject.SetActive(false);
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

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, basicAttackRange);
        Gizmos.DrawWireSphere(attackPoint.position, iceAttackRange);
        Gizmos.DrawWireSphere(attackPoint.position, fireAttackRange);
    }
}
