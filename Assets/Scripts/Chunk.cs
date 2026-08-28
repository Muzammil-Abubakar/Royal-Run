using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private GameObject fence;

    private readonly float[] fencePositions = { -2.5f, 0f, 2.5f };

    private void Start()
    {
        GenerateFences();
    }

    private void GenerateFences()
    {
        // Randomly choose 0, 1, or 2 fences
        int fenceCount = Random.Range(0, 3);

        // Keep track of which positions have already been used
        bool[] usedPositions = new bool[fencePositions.Length];

        for (int i = 0; i < fenceCount; i++)
        {
            int randomIndex;

            do
            {
                randomIndex = Random.Range(0, fencePositions.Length);
            }
            while (usedPositions[randomIndex]);

            usedPositions[randomIndex] = true;

            // Position relative to the Chunk
            Vector3 spawnPosition = transform.position;
            spawnPosition.x += fencePositions[randomIndex];

            // Spawn the fence
            Instantiate(fence, spawnPosition, Quaternion.identity, transform);
        }
    }
}