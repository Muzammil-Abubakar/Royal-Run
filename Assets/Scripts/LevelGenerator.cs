using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject chunkPrefab;
    [SerializeField] private Transform chunkParent;
    [SerializeField] private int totalChunks = 10;
    [SerializeField] private float chunkDistance = 10f;

    void Start()
    {
        GenerateChunks();
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

            Instantiate(chunkPrefab, spawnPosition, Quaternion.identity, chunkParent);
        }
    }
}