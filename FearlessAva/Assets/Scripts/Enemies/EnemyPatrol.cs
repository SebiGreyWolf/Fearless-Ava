using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header ("Patrol Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Enemy")]
    [SerializeField] private Transform enemy;


    [Header("Movement")]
    [SerializeField] private float speed = 2.5f;
    private Vector3 initScale;
    private bool movingLeft = true;

    [Header("Idleing")]
    [SerializeField] private float idleDuration = 1;
    private float idleTimer;


    [Header("Animator")]
    [SerializeField] private Animator animator;


    private void Awake()
    {
        initScale = enemy.transform.localScale;
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
        //animator.SetBool("EnemyWalking", true);

        idleTimer = 0;

        enemy.localScale = new Vector3 (Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * speed, enemy.position.y, enemy.position.z);
    }



}
