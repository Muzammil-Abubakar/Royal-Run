using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private float spawnRange = 3.5f;

    private void Start()
    {
        StartCoroutine(SpawnObstacles());
    }

    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            // Wait a random amount of time between 1 and 2 seconds
            yield return new WaitForSeconds(Random.Range(0.3f, 0.6f));

            // Random X position relative to this object's position
            float randomX = Random.Range(-spawnRange, spawnRange);

            Vector3 spawnPosition = transform.position;
            spawnPosition.x += randomX;

            // Choose a random obstacle
            GameObject obstacle = obstacles[Random.Range(0, obstacles.Length)];

            // Random rotation on all axes
            Quaternion randomRotation = Quaternion.Euler(
                Random.Range(0f, 360f),
                Random.Range(0f, 360f),
                Random.Range(0f, 360f)
            );

            // Spawn the obstacle
            Instantiate(obstacle, spawnPosition, randomRotation);
        }
    }
}