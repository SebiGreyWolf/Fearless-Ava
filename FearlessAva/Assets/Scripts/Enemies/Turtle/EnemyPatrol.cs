using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;
    private SpriteRenderer spriteRenderer;

    [Header("Movement")]
    [SerializeField] private float speed = 2.5f;
    private float baseSpeed;
    private Vector3 initScale;
    private bool movingLeft = true;
    public bool isSlowed = false;
    private float slowTimer = 0f;
    private bool wasSlowedBefore = false;
    private float freezeTimer = 0f;
    public float freezeDuration = 3f;
    private bool isFrozen = false;

    [Header("Idleing")]
    [SerializeField] private float idleDuration = 1;
    private float idleTimer;

    public Animator animation;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animation = GetComponentInChildren<Animator>();
        initScale = enemy.transform.localScale;
        baseSpeed = speed;
    }


    private void Update()
    {
        if(!enemy.IsDestroyed())
        {
            if (movingLeft)
            {
                if (enemy.position.x >= leftEdge.position.x)
                    MoveInDirection(-1);
                else
                    DirectionChange();
            }
            else
            {
                if (enemy.position.x <= rightEdge.position.x)
                    MoveInDirection(1);
                else
                    DirectionChange();
            }

            if (isSlowed)
            {
                spriteRenderer.color = Color.blue;
                slowTimer -= Time.deltaTime;
                if (slowTimer <= 0f)
                {
                    isSlowed = false;
                    speed *= 2;
                    spriteRenderer.color = Color.white;
                }
            }
            else if (isFrozen)
            {
                spriteRenderer.color = Color.blue;
                freezeTimer -= Time.deltaTime;
                if (freezeTimer <= 0f)
                {
                    isFrozen = false;
                    speed = baseSpeed;
                    spriteRenderer.color = Color.white;
                }
            }
        }
    }

    private void DirectionChange()
    {
        idleTimer += Time.deltaTime;

        if(idleTimer > idleDuration)
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int direction)
    {
        idleTimer = 0;

        enemy.localScale = new Vector3 (Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * speed, enemy.position.y, enemy.position.z);

        animation.SetFloat("EnemyWalkingSpeed", speed);
    }

    public void ApplySlow(float slowDuration)
    {
        if (!wasSlowedBefore)
        {
            if (!isSlowed)
            {
                isSlowed = true;
                slowTimer = slowDuration;
                speed /= 2;
                wasSlowedBefore = true;
            }
        }
        else
        {
            wasSlowedBefore = false;
            isFrozen = true;
            freezeTimer = freezeDuration;
            speed = 0;
        }
        
    }

}
