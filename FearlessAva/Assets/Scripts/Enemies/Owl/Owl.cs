using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Owl : MonoBehaviour
{
    public Player player;

    private Vector3 startPoint;
    private Vector3 endPoint;
    private float curveHeight = 5;

    private float duration = 5f;
    private float timeElapsed;
    private float lifeSpan = 3f;

    void Start()
    {
        startPoint = new Vector3(player.transform.position.x + 30, player.transform.position.y + curveHeight, player.transform.position.z);
        endPoint = new Vector3(player.transform.position.x - 30, player.transform.position.y + curveHeight, player.transform.position.z);
    }


    void Update()
    {
        timeElapsed += Time.deltaTime;

        if(timeElapsed > lifeSpan)
        {
            Destroy(gameObject);
        }

        transform.position = MathParabola.Parabola(startPoint, endPoint, -curveHeight, timeElapsed / duration);

    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(5);
            Destroy(gameObject);
        }
    }
}
