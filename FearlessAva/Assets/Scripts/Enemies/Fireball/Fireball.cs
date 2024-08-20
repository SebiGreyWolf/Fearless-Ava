using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Player player;
    public float distanceToDespawn;
    public int damage;
    public int direction = 1;
    public float speed;

    Vector3 startPoint;


    private void Awake()
    {
        startPoint = transform.position;

        //setDirection();
    }

    void Update()
    {
        float currentSpeed = speed;
        float distance = Vector3.Distance(transform.position, player.transform.position);
        if(distance > 30 )
        {
            currentSpeed = speed * 2;
        }

        transform.position = new Vector3(transform.position.x + currentSpeed * direction , transform.position.y, transform.position.z);

        if (isFireballTooFarFromSpawn())
        {
            Destroy(gameObject);
        }
    }

    bool isFireballTooFarFromSpawn()
    {
        if (this != null && player != null)
        {
            float distance = Vector3.Distance(transform.position, startPoint);

            if (distance >= distanceToDespawn)
            {
                return true;
            }
        }
        return false;
    }

    void setDirection()
    {
        if (player.transform.position.x < startPoint.x)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
