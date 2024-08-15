using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    public Player player;
    public Dragon dragon;

    public float fallingSpeed;
    private bool isFalling = false;


    // Update is called once per frame
    void Update()
    {
        if (isFalling)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - fallingSpeed, transform.position.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
        {
            isFalling = true;
        }

        if (collision.gameObject.CompareTag("Dragon"))
        {
            dragon.CollisionWithRock();

            //Maybe add some particle Effects here
            Destroy(gameObject);
        }

        
    }
}
