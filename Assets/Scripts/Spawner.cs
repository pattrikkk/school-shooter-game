using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public Target aiPrefab;       // The AI prefab to spawn
    public float spawnInterval = 2f;  // Time between each spawn
    public Vector3 spawnAreaMin;      // The minimum point of the spawn area
    public Vector3 spawnAreaMax;      // The maximum point of the spawn area
    public Transform playerr;
    private void Start()
    {
        // Start the spawn process
        InvokeRepeating("SpawnAI", 0f, spawnInterval);
    }

    void SpawnAI()
    {
        // Generate a random position within the spawn area
        float randomX = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float randomY = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        float randomZ = Random.Range(spawnAreaMin.z, spawnAreaMax.z);

        Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);

        // Instantiate the AI prefab at the random position
        var ai = Instantiate(aiPrefab, spawnPosition, Quaternion.identity);
        ai.Setup(playerr);
    }
}
