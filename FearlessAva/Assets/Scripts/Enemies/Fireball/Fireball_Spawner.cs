using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class FireballSpawner : MonoBehaviour
{
    public Player player;

    public GameObject fireball;
    public Vector3[] spawnPositions;
    public float delayInSeconds;
    private float timeSinceLastSpawn = 0;
    public float detectionRange;

    void Start()
    {
        
    }

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
