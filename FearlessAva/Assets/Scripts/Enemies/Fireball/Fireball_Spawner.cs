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


    public int minSecBetweenFireballs;
    public int maxSecBetweenFireballs;


    System.Random rand = new System.Random();
    private int[] times = {0,0,0};

    private void Awake()
    {
        for (int i = 0; i < times.Length; i++)
        {
            times[i] = rand.Next(minSecBetweenFireballs, maxSecBetweenFireballs);
        }
    }

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
                    GameObject newFireball = Instantiate(fireball, spawnPositions[i], Quaternion.identity);
                    newFireball.SetActive(true);
                    timeSinceLastSpawn[i] = 0;
                    times[i] = rand.Next(minSecBetweenFireballs, maxSecBetweenFireballs);
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
