using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingSoundManager : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<Player>() != null)
        {
            FindObjectOfType<AudioManagement>().PlaySound("Busch");
        }
    }
}
