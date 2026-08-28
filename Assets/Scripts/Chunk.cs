
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private GameObject fence;
    [SerializeField] private GameObject apple;
    [SerializeField] private GameObject coin;

    [SerializeField, Range(0f, 1f)]
    private float pickupChance = 0.75f;

    private readonly float[] lanePositions = { -2.5f, 0f, 2.5f };

    private void Start()
    {
        bool[] occupiedLanes = GenerateFences();

        GeneratePickup(occupiedLanes);
    }

    private bool[] GenerateFences()
    {
        // Randomly choose 0, 1, or 2 fences
        int fenceCount = Random.Range(0, 3);

        bool[] occupiedLanes = new bool[lanePositions.Length];

        for (int i = 0; i < fenceCount; i++)
        {
            int randomIndex;

            do
            {
                randomIndex = Random.Range(0, lanePositions.Length);
            }
            while (occupiedLanes[randomIndex]);

            occupiedLanes[randomIndex] = true;

            Vector3 spawnPosition = transform.position;
            spawnPosition.x += lanePositions[randomIndex];

            Instantiate(fence, spawnPosition, Quaternion.identity, transform);
        }

        return occupiedLanes;
    }

    private void GeneratePickup(bool[] occupiedLanes)
    {
        // Don't spawn a pickup on every chunk
        if (Random.value > pickupChance)
            return;

        // Find free lanes
        int[] freeLanes = new int[lanePositions.Length];
        int freeLaneCount = 0;

        for (int i = 0; i < lanePositions.Length; i++)
        {
            if (!occupiedLanes[i])
            {
                freeLanes[freeLaneCount] = i;
                freeLaneCount++;
            }
        }

        // No free lanes
        if (freeLaneCount == 0)
            return;

        // Choose ONE free lane
        int selectedLane = freeLanes[Random.Range(0, freeLaneCount)];

        // 50/50 chance of apple or coins
        if (Random.value < 0.5f)
        {
            GenerateCoins(selectedLane);
        }
        else
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.x += lanePositions[selectedLane];

            Instantiate(apple, spawnPosition, Quaternion.identity, transform);
        }
    }

    private void GenerateCoins(int selectedLane)
    {
        const float startZ = -4f;
        const float endZ = 4f;
        const float coinGap = 0.7f;

        // Calculate how many coin positions fit between -4 and +4
        int maxCoins = Mathf.FloorToInt((endZ - startZ) / coinGap) + 1;

        // Minimum of 4 coins, maximum based on available space
        int coinCount = Random.Range(4, maxCoins + 1);

        // Pick a random starting point between -4 and +4
        // while ensuring the entire coin chain stays inside the limits.
        int maxStartIndex = maxCoins - coinCount;

        int startIndex = Random.Range(0, maxStartIndex + 1);

        float actualStartZ = startZ + (startIndex * coinGap);

        for (int i = 0; i < coinCount; i++)
        {
            Vector3 spawnPosition = transform.position;

            // Stay in the selected free lane
            spawnPosition.x += lanePositions[selectedLane];

            // Spawn coins along the Z axis
            spawnPosition.z += actualStartZ + (i * coinGap);

            Instantiate(coin, spawnPosition, Quaternion.identity, transform);
        }
    }
}
