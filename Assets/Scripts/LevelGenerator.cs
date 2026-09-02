using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Chunk Generation")]
    [Tooltip("The prefab used to create each level chunk.")]
    [SerializeField] private GameObject chunkPrefab;

    [Tooltip("The parent Transform that all generated chunks will be placed under.")]
    [SerializeField] private Transform chunkParent;

    [Tooltip("The distance between each generated chunk along the Z axis.")]
    [SerializeField] private float chunkDistance = 10f;

    [Tooltip("The total number of chunks generated when the level starts.")]
    [SerializeField] private int totalChunks = 20;


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


    void Start()
    {
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


    void MoveChunks()
    {
        foreach (GameObject chunk in chunks)
        {
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
        float furthestZ =
            chunks[chunks.Count - 1].transform.position.z;

        Vector3 spawnPosition = new Vector3(
            0f,
            0f,
            furthestZ + chunkDistance
        );

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

