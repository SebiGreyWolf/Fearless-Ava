using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public float changeTime = 0.05f;
    public float restoreSpeed = 10f;
    public float delay = 0.1f;
    public ParticleSystem hitParticles;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerMovement>() != null)
        {
            hitParticles.Play();
            collision.gameObject.GetComponent<TimeStop>().StopTime(changeTime, restoreSpeed, delay);
        }
    }
}
