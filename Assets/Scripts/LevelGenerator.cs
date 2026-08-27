using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private Transform chunkParent;
    [SerializeField] private Transform cameraTransform;

    [SerializeField] private int totalChunks = 10;
    [SerializeField] private float chunkDistance = 10f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float removalOffset = 10f;

    private List<GameObject> chunks = new List<GameObject>();

    void Start()
    {
        GenerateChunks();
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
        }
    }

    void MoveChunks()
    {
        // Move all chunks backward.
        foreach (GameObject chunk in chunks)
        {
            chunk.transform.Translate(
                Vector3.back * moveSpeed * Time.deltaTime,
                Space.World
            );
        }

        // Remove chunks that have gone behind the camera.
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
        // The last element is always the furthest-forward chunk.
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
    }
}