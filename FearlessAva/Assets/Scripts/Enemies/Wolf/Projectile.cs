using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Player player;
    private float travelTime;


    public Vector3 startPoint;
    public Vector3 endPoint;
    public float duration = 1f;
    public float curveHeight = 2f;

    private float timeElapsed;
    private float lifeSpan = 3f;

    private void Start()
    {
        startPoint = transform.position;
        endPoint = player.transform.position;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        if(timeElapsed > lifeSpan)
        {
            Destroy(gameObject);
        }

        Throw();
    }

    public void Throw()
    {
        travelTime += Time.deltaTime;
        //travelTime %= duration;

        transform.position = MathParabola.Parabola(startPoint, endPoint, curveHeight, travelTime / duration);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(2);
            Destroy(gameObject);
        }
    }
}
