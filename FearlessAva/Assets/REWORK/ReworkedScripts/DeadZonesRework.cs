using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZonesRework : MonoBehaviour
{
    public GameObject spawnPoint;
    public float changeTime = 0.05f;
    public float restoreSpeed = 10f;
    public float delay = 0.1f;
    public ParticleSystem hitParticles;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<ReworkedPlayerMovement>() != null)
        {
            hitParticles.Play();
            collision.gameObject.GetComponent<TimeStop>().StopTime(changeTime, restoreSpeed, delay);
            collision.gameObject.transform.position = spawnPoint.transform.position;
        }
        
    }
}
