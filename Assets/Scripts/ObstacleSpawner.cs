using UnityEngine;
using System.Collections;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] private GameObject obstacle;

    private void Start()
    {
        StartCoroutine(SpawnObstacles());
    }

    private IEnumerator SpawnObstacles()
    {
        while (true)
        {
            // Wait a random amount of time between 1 and 2 seconds
            yield return new WaitForSeconds(Random.Range(1f, 2f));

            // Random X position relative to this object's position
            float randomX = Random.Range(-4.5f, 4.5f);

            Vector3 spawnPosition = transform.position;
            spawnPosition.x += randomX;

            // Spawn the obstacle
            Instantiate(obstacle, spawnPosition, Quaternion.identity);
        }
    }
}