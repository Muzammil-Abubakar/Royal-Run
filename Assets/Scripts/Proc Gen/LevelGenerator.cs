
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Chunk Generation")]
    [Tooltip("Normal level chunks. One of these will be randomly selected whenever a normal chunk is generated.")]
    [SerializeField] private GameObject[] normalChunks;

    [Tooltip("The special checkpoint chunk.")]
    [SerializeField] private GameObject checkpointChunk;

    [Tooltip("The parent Transform that all generated chunks will be placed under.")]
    [SerializeField] private Transform chunkParent;

    [Tooltip("The distance between each generated chunk along the Z axis.")]
    [SerializeField] private float chunkDistance = 10f;

    [Tooltip("The total number of chunks generated when the level starts.")]
    [SerializeField] private int totalChunks = 20;


    [Header("Checkpoint Generation")]
    [Tooltip("The initial chance of spawning a checkpoint.")]
    [Range(0f, 100f)]
    [SerializeField] private float checkpointChance = 2f;

    [Tooltip("How much the checkpoint chance increases after a failed checkpoint roll.")]
    [Range(0f, 100f)]
    [SerializeField] private float checkpointChanceIncrease = 2f;

    [Tooltip("Minimum number of chunks that must spawn between checkpoints.")]
    [SerializeField] private int minimumCheckpointDistance = 3;

    [Tooltip("Maximum number of eligible chunks allowed between checkpoints. The checkpoint becomes guaranteed after this point.")]
    [SerializeField] private int maximumCheckpointDistance = 10;


    [Header("Starting Area")]
    [Tooltip("The number of starting chunks that will remain empty before chunk spawning begins.")]
    [SerializeField] private int emptyStartingChunks = 12;


    [Header("Movement")]
    [Tooltip("The current speed at which all chunks move toward the player.")]
    [SerializeField] private float moveSpeed = 16f;

    [Tooltip("The minimum speed that the level can move at.")]
    [SerializeField] private float minMoveSpeed = 8f;

    [Tooltip("The maximum speed that the level can move at.")]
    [SerializeField] private float maxMoveSpeed = 24f;


    [Header("Camera")]
    [Tooltip("The camera controller used to keep the camera's movement speed synchronized with the level speed.")]
    [SerializeField] private CameraController cameraController;


    [Header("Score")]
    [Tooltip("The scoreboard used to track the player's score.")]
    [SerializeField] private Scoreboard scoreboard;


    [Header("Chunk Cleanup")]
    [Tooltip("The Transform of the camera used to determine when chunks have moved behind the player.")]
    [SerializeField] private Transform cameraTransform;

    [Tooltip("The additional distance behind the camera a chunk must travel before it is removed.")]
    [SerializeField] private float removalOffset = 10f;


    private List<GameObject> chunks = new List<GameObject>();

    private float currentCheckpointChance;
    private int chunksSinceCheckpoint;


    void Start()
    {
        currentCheckpointChance = checkpointChance;

        GenerateChunks();

        UpdateCameraSpeed();
    }


    void Update()
    {
        MoveChunks();
    }


    void GenerateChunks()
    {
        for (int i = 0; i < totalChunks; i++)
        {
            Vector3 spawnPosition = new Vector3(
                0f,
                0f,
                i * chunkDistance
            );

            GameObject chunkPrefab = GetChunkPrefab(i);

            if (chunkPrefab == null)
            {
                Debug.LogError("LevelGenerator: No valid chunk prefab was found.");
                continue;
            }

            GameObject chunk = Instantiate(
                chunkPrefab,
                spawnPosition,
                Quaternion.identity,
                chunkParent
            );

            chunks.Add(chunk);

            Chunk chunkScript = chunk.GetComponent<Chunk>();

            if (chunkScript != null)
            {
                bool allowSpawning = i >= emptyStartingChunks;

                chunkScript.Initialize(
                    allowSpawning,
                    this,
                    scoreboard
                );
            }
        }
    }


    GameObject GetChunkPrefab(int chunkIndex)
    {
        if (chunkIndex < emptyStartingChunks)
        {
            return GetRandomNormalChunk();
        }

        return GetRandomChunk();
    }


    GameObject GetRandomChunk()
    {
        // Prevent checkpoints from spawning too close together.
        if (chunksSinceCheckpoint < minimumCheckpointDistance)
        {
            chunksSinceCheckpoint++;

            return GetRandomNormalChunk();
        }

        // Guarantee a checkpoint once the maximum distance is reached.
        if (chunksSinceCheckpoint >= maximumCheckpointDistance)
        {
            SpawnedCheckpoint();

            return checkpointChunk;
        }

        // Roll for a checkpoint.
        float roll = Random.Range(0f, 100f);

        if (roll < currentCheckpointChance)
        {
            SpawnedCheckpoint();

            return checkpointChunk;
        }

        // No checkpoint this time.
        chunksSinceCheckpoint++;

        // Gradually increase the chance.
        currentCheckpointChance = Mathf.Min(
            currentCheckpointChance + checkpointChanceIncrease,
            100f
        );

        return GetRandomNormalChunk();
    }


    GameObject GetRandomNormalChunk()
    {
        if (normalChunks == null || normalChunks.Length == 0)
        {
            Debug.LogError("LevelGenerator: No normal chunks have been assigned.");

            return null;
        }

        return normalChunks[
            Random.Range(0, normalChunks.Length)
        ];
    }


    void SpawnedCheckpoint()
    {
        chunksSinceCheckpoint = 0;

        currentCheckpointChance = checkpointChance;
    }


    void MoveChunks()
    {
        foreach (GameObject chunk in chunks)
        {
            if (chunk == null)
            {
                continue;
            }

            chunk.transform.Translate(
                Vector3.back * moveSpeed * Time.deltaTime,
                Space.World
            );
        }

        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            GameObject chunk = chunks[i];

            if (chunk == null)
            {
                chunks.RemoveAt(i);

                continue;
            }

            if (chunk.transform.position.z <
                cameraTransform.position.z - removalOffset)
            {
                chunks.RemoveAt(i);

                Destroy(chunk);

                SpawnChunkAtFront();
            }
        }
    }


    void SpawnChunkAtFront()
    {
        if (chunks.Count == 0)
        {
            return;
        }

        float furthestZ =
            chunks[chunks.Count - 1].transform.position.z;

        Vector3 spawnPosition = new Vector3(
            0f,
            0f,
            furthestZ + chunkDistance
        );

        GameObject chunkPrefab = GetRandomChunk();

        if (chunkPrefab == null)
        {
            return;
        }

        GameObject newChunk = Instantiate(
            chunkPrefab,
            spawnPosition,
            Quaternion.identity,
            chunkParent
        );

        chunks.Add(newChunk);

        Chunk chunkScript =
            newChunk.GetComponent<Chunk>();

        if (chunkScript != null)
        {
            chunkScript.Initialize(
                true,
                this,
                scoreboard
            );
        }
    }


    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed = Mathf.Clamp(
            moveSpeed + amount,
            minMoveSpeed,
            maxMoveSpeed
        );

        UpdateCameraSpeed();
    }


    public void DecreaseMoveSpeed(float amount)
    {
        moveSpeed = Mathf.Clamp(
            moveSpeed - amount,
            minMoveSpeed,
            maxMoveSpeed
        );

        UpdateCameraSpeed();
    }


    private void UpdateCameraSpeed()
    {
        cameraController.SetSpeed(
            moveSpeed,
            minMoveSpeed,
            maxMoveSpeed
        );
    }
}

