using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwlSpawner : MonoBehaviour
{
    public int owlsToSpawn = 3;
    public int owlsSpawnedAlready = 0;

    private float spawnCooldown = 2f;
    private float cooldownTimer = 1f;

    private bool spawnerTriggered = false;

    public GameObject owl;


    void Update()
    {
        cooldownTimer += Time.deltaTime;

        if (cooldownTimer >= spawnCooldown && spawnerTriggered && !allOwlsSpawned())
        {
            cooldownTimer = 0;
            owlsSpawnedAlready++;
            GameObject newOwl = Instantiate(owl, transform.position, Quaternion.identity);
            newOwl.SetActive(true);
        }
    }

    private bool allOwlsSpawned()
    {
        if(owlsSpawnedAlready < owlsToSpawn)
            return false;

        Destroy(gameObject);
        return true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            spawnerTriggered = true;
        }
    }
}