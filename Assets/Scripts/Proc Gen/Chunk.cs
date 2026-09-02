using UnityEngine;

public class Chunk : MonoBehaviour
{
    [Header("Chunk Spawning")]
    [SerializeField] private GameObject fence;
    [SerializeField] private GameObject apple;
    [SerializeField] private GameObject coin;

    [SerializeField, Range(0f, 1f)]
    private float pickupChance = 0.75f;

    [Header("Floor Collider")]
    [SerializeField] private Collider floorCollider;

    [Header("Obstacle Force")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float forceAmount = 2f;
    [SerializeField] private float forceInterval = 1f;

    private readonly float[] lanePositions = { -2.5f, 0f, 2.5f };

    private bool spawningEnabled = false;

    private LevelGenerator levelGenerator;
    private Scoreboard scoreboard;

    private void Start()
    {
        InvokeRepeating(
            nameof(ApplyForceToObstacles),
            forceInterval,
            forceInterval
        );
    }

    public void Initialize(
        bool allowSpawning,
        LevelGenerator levelGenerator,
        Scoreboard scoreboard)
    {
        spawningEnabled = allowSpawning;

        this.levelGenerator = levelGenerator;
        this.scoreboard = scoreboard;

        if (!spawningEnabled)
            return;

        bool[] occupiedLanes = GenerateFences();

        GeneratePickup(occupiedLanes);
    }

    private void ApplyForceToObstacles()
    {
        if (floorCollider == null)
            return;

        Bounds bounds = floorCollider.bounds;

        Collider[] obstacles = Physics.OverlapBox(
            bounds.center,
            bounds.extents,
            Quaternion.identity,
            obstacleLayer
        );

        foreach (Collider obstacle in obstacles)
        {
            Rigidbody rb = obstacle.attachedRigidbody;

            if (rb == null)
                continue;

            rb.AddForce(
                Vector3.up * forceAmount,
                ForceMode.Impulse
            );
        }
    }

    private bool[] GenerateFences()
    {
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

            Instantiate(
                fence,
                spawnPosition,
                Quaternion.identity,
                transform
            );
        }

        return occupiedLanes;
    }

    private void GeneratePickup(bool[] occupiedLanes)
    {
        if (Random.value > pickupChance)
            return;

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

        if (freeLaneCount == 0)
            return;

        int selectedLane =
            freeLanes[Random.Range(0, freeLaneCount)];

        if (Random.value < 0.5f)
        {
            GenerateCoins(selectedLane);
        }
        else
        {
            Vector3 spawnPosition = transform.position;
            spawnPosition.x += lanePositions[selectedLane];

            GameObject appleObject = Instantiate(
                apple,
                spawnPosition,
                Quaternion.identity,
                transform
            );

            Apple applePickup =
                appleObject.GetComponent<Apple>();

            applePickup.Init(levelGenerator);
        }
    }

    private void GenerateCoins(int selectedLane)
    {
        const float startZ = -4f;
        const float endZ = 4f;
        const float coinGap = 0.7f;

        int maxCoins = Mathf.FloorToInt(
            (endZ - startZ) / coinGap
        ) + 1;

        int coinCount = Random.Range(
            4,
            maxCoins + 1
        );

        int maxStartIndex = maxCoins - coinCount;

        int startIndex = Random.Range(
            0,
            maxStartIndex + 1
        );

        float actualStartZ =
            startZ + (startIndex * coinGap);

        for (int i = 0; i < coinCount; i++)
        {
            Vector3 spawnPosition = transform.position;

            spawnPosition.x +=
                lanePositions[selectedLane];

            spawnPosition.z +=
                actualStartZ + (i * coinGap);

            GameObject coinObject = Instantiate(
                coin,
                spawnPosition,
                Quaternion.identity,
                transform
            );

            Coin coinPickup =
                coinObject.GetComponent<Coin>();

            coinPickup.Init(scoreboard);
        }
    }

    private void OnDestroy()
    {
        CancelInvoke(nameof(ApplyForceToObstacles));
    }
}

