using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingRock : MonoBehaviour
{
    public Player player;
    public Dragon dragon;
    public Material mat;
    public Material originalMat;
    public SpriteRenderer renderer;


    public float fallingSpeed;
    private bool isFalling = false;
    private bool isPlayerInTrigger;


    // Update is called once per frame
    void Update()
    {
        if (isPlayerInTrigger)
        {
            renderer.material = mat;

            if (Input.GetKeyUp(KeyCode.F) && isPlayerInTrigger)
            {
                isFalling = true;
            }
        }
        else
        {
            renderer.material = originalMat;
        }


        if (isFalling)
        {
            FindObjectOfType<AudioManagement>().PlaySound("StoneFall");
            transform.position = new Vector3(transform.position.x, transform.position.y - fallingSpeed, transform.position.z);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            isPlayerInTrigger = true;
        }
        else if (collision.gameObject.CompareTag("Dragon"))
        {
            dragon.CollisionWithRock();

            //Maybe add some particle Effects here
            Destroy(gameObject);
        }
    }

    // Detect when the player exits the trigger zone
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Player>())
        {
            isPlayerInTrigger = false;
        }
    }
}
