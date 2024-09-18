using System.Collections;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    // Public prefab variables
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;

    // Minimum and maximum spawn interval (in seconds)
    public float minSpawnTime = 10f;
    public float maxSpawnTime = 15f;

    // Start is called before the first frame update
    void Start()
    {
        // Start spawning objects
        StartCoroutine(SpawnPrefab());
    }

    // Coroutine to handle the spawning logic
    IEnumerator SpawnPrefab()
    {
        while (true) // Infinite loop to keep spawning
        {
            // Randomize the spawn interval
            float waitTime = Random.Range(minSpawnTime, maxSpawnTime);
            yield return new WaitForSeconds(waitTime);

            // Randomly pick one of the prefabs
            int randomIndex = Random.Range(0, 3);

            // Choose the prefab based on the random index
            GameObject prefabToSpawn = null;
            switch (randomIndex)
            {
                case 0:
                    prefabToSpawn = prefab1;
                    break;
                case 1:
                    prefabToSpawn = prefab2;
                    break;
                case 2:
                    prefabToSpawn = prefab3;
                    break;
            }

            // Instantiate the chosen prefab at this GameObject's position and rotation
            if (prefabToSpawn != null)
            {
                Instantiate(prefabToSpawn, transform.position, transform.rotation);
            }
        }
    }
}
