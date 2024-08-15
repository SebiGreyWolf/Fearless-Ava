using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class FireballSpawner : MonoBehaviour
{
    public Player player;
    public Dragon dragon;

    public GameObject fireball;
    public Vector3[] spawnPositions;
    public float delayInSeconds;
    public float detectionRange;
    private float[] timeSinceLastSpawn = {0,0,0};


    System.Random rand = new System.Random();
    private int[] times = {0,0,0};

    private void Awake()
    {
        times[0] = rand.Next(1, 5);
        times[1] = rand.Next(1, 5);
        times[2] = rand.Next(1, 5);
    }

    /*
    void Update()
    {
        if (isPlayerInRange())
        {
            float minDistance = Vector3.Distance(spawnPositions[0], player.transform.position);
            Vector3 vec = spawnPositions[0];
            foreach (var spawn in spawnPositions)
            {
                if (minDistance > Vector3.Distance(spawn, player.transform.position))
                {
                    minDistance = Vector3.Distance(spawn, player.transform.position);
                    vec = spawn;
                }
            }

            timeSinceLastSpawn += Time.deltaTime;

            if (timeSinceLastSpawn > delayInSeconds)
            {
                GameObject newFireball = Instantiate(fireball, vec, Quaternion.identity);
                newFireball.SetActive(true);
                timeSinceLastSpawn = 0;
            }
        }
    }*/

    private void Update()
    {
        for (int i = 0; i < timeSinceLastSpawn.Length;i++)
        {
            timeSinceLastSpawn[i] += Time.deltaTime;
        }

        if (isPlayerInRange() && !dragon.IsDestroyed())
        {
            for (int i = 0; i < timeSinceLastSpawn.Length; i++)
            {
                if (timeSinceLastSpawn[i] > times[i])
                {
                    GameObject newFireball = Instantiate(fireball, spawnPositions[0], Quaternion.identity);
                    newFireball.SetActive(true);
                    timeSinceLastSpawn[i] = 0;
                    times[i] = rand.Next(1, 5);
                }
            }
        }
    }

    bool isPlayerInRange()
    {
        if (this != null && player != null)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);

            if (distance <= detectionRange)
            {
                return true;
            }
        }
        return false;
    }
}
