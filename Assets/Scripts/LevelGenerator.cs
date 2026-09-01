using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Chunk Generation")]
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private Transform chunkParent;
    [SerializeField] private float chunkDistance = 10f;
    [SerializeField] private int totalChunks = 20;

    [Header("Starting Area")]
    [SerializeField] private int emptyStartingChunks = 12;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 16f;
    [SerializeField] private float minMoveSpeed = 8f;
    [SerializeField] private float maxMoveSpeed = 24f;

    [Header("Camera")]
    [SerializeField] private CameraController cameraController;

    [Header("Chunk Cleanup")]
    [SerializeField] private Transform cameraTransform;
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
                chunkScript.Initialize(allowSpawning);
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
        float furthestZ = chunks[chunks.Count - 1].transform.position.z;

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

        Chunk chunkScript = newChunk.GetComponent<Chunk>();

        if (chunkScript != null)
        {
            chunkScript.Initialize(true);
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