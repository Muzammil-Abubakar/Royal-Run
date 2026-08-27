using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private Transform chunkParent;
    [SerializeField] private int totalChunks = 10;
    [SerializeField] private float chunkDistance = 10f;

    [SerializeField] private float moveSpeed = 5f;

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
        foreach (GameObject chunk in chunks)
        {
            chunk.transform.Translate(
                Vector3.back * moveSpeed * Time.deltaTime,
                Space.World
            );
        }
    }
}