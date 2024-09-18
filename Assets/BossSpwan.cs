using UnityEngine;
using System.Collections;

public class BossSpawn : MonoBehaviour
{
    public GameObject prefabToSpawn;  // Reference to the prefab you want to spawn
    public float delay = 120f;         // Time in seconds to wait before spawning (2 minutes)

    private bool hasSpawned = false;  // To ensure the prefab is spawned only once

    void Start()
    {
        // Start the coroutine to handle the delayed spawning
        StartCoroutine(SpawnPrefabAfterDelay());
    }

    IEnumerator SpawnPrefabAfterDelay()
    {
        // Wait for the specified delay time
        yield return new WaitForSeconds(delay);

        // Check if the prefab has not been spawned already
        if (!hasSpawned)
        {
            // Spawn the prefab at the position and rotation of this GameObject
            Instantiate(prefabToSpawn, transform.position, transform.rotation);

            // Set hasSpawned to true to ensure it only spawns once
            hasSpawned = true;
        }
    }
}

