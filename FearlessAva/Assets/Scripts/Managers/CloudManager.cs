using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudManager : MonoBehaviour
{
    public GameObject[] cloudPrefabs; // Array to hold the different cloud prefabs
    public Transform spawnPoint; // Point to spawn clouds on the right
    public Transform endPoint; // Point where clouds will be repositioned

    public int initialCloudCount = 5; // Number of clouds to spawn initially
    public float cloudSpeed = 2f; // Speed of cloud movement

    public float minScale = 0.5f; // Minimum scale factor for clouds
    public float maxScale = 1.5f; // Maximum scale factor for clouds
    public float yOffsetRange = 2f; // Range of Y offset for cloud positions

    private List<GameObject> clouds = new List<GameObject>();

    void Start()
    {
        SpawnInitialClouds();
    }

    void Update()
    {
        MoveClouds();
    }

    void SpawnInitialClouds()
    {
        for (int i = 0; i < initialCloudCount; i++)
        {
            float randomX = Random.Range(spawnPoint.position.x, endPoint.position.x);
            float randomY = Random.Range(spawnPoint.position.y - yOffsetRange, spawnPoint.position.y + yOffsetRange);
            SpawnCloud(new Vector3(randomX, randomY, 0));
        }
    }

    void SpawnCloud(Vector3 position)
    {
        int randomIndex = Random.Range(0, cloudPrefabs.Length);
        GameObject cloud = Instantiate(cloudPrefabs[randomIndex], position, Quaternion.identity);
        cloud.transform.localScale = Vector3.one * Random.Range(minScale, maxScale); // Set random scale
        clouds.Add(cloud);
    }

    void MoveClouds()
    {
        foreach (GameObject cloud in clouds)
        {
            cloud.transform.position += Vector3.left * cloudSpeed * Time.deltaTime;

            if (cloud.transform.position.x <= endPoint.position.x)
            {
                RepositionCloud(cloud);
            }
        }
    }

    void RepositionCloud(GameObject cloud)
    {
        float randomY = Random.Range(spawnPoint.position.y - yOffsetRange, spawnPoint.position.y + yOffsetRange);
        cloud.transform.position = new Vector3(spawnPoint.position.x, randomY, 0);
        cloud.transform.localScale = Vector3.one * Random.Range(minScale, maxScale); // Set random scale
    }
}
