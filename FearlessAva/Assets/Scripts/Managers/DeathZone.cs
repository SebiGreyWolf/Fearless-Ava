using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public Transform respawnPoint;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Get in here");
        if (collision.GetComponent<Player>())
        {
            Debug.Log("I am the Player");
            collision.gameObject.transform.position = respawnPoint.transform.position;
        }
        //Maybe let enemy who falls into void destroy
    }
}
