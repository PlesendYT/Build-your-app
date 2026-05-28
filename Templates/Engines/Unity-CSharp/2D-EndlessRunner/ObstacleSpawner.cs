using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] obstaclePrefabs;
    public GameObject[] collectiblePrefabs;
    public float spawnInterval = 1.5f;
    public float minSpawnInterval = 0.6f;
    public float difficultyIncreaseRate = 0.01f;
    public float spawnXPosition = 12f;
    public float spawnYMin = -2f;
    public float spawnYMax = 2f;

    private float currentInterval;
    private float nextSpawnTime;

    private void Start()
    {
        currentInterval = spawnInterval;
        nextSpawnTime = Time.time + currentInterval;
    }

    private void Update()
    {
        currentInterval = Mathf.Max(minSpawnInterval, spawnInterval - Time.timeSinceLevelLoad * difficultyIncreaseRate);

        if (Time.time >= nextSpawnTime)
        {
            Spawn();
            nextSpawnTime = Time.time + currentInterval;
        }
    }

    private void Spawn()
    {
        float yPos = Random.Range(spawnYMin, spawnYMax);
        Vector3 spawnPos = new Vector3(spawnXPosition, yPos, 0);

        if (Random.value < 0.7f)
        {
            if (obstaclePrefabs.Length > 0)
            {
                GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                Instantiate(prefab, spawnPos, Quaternion.identity);
            }
        }
        else
        {
            if (collectiblePrefabs.Length > 0)
            {
                GameObject prefab = collectiblePrefabs[Random.Range(0, collectiblePrefabs.Length)];
                Instantiate(prefab, spawnPos + Vector3.up, Quaternion.identity);
            }
        }
    }
}
