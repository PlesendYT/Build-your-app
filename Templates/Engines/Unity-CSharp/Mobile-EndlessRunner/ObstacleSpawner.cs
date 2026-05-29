using UnityEngine;

/// <summary>
/// Spawns obstacles at timed intervals in random lanes for the endless runner.
/// </summary>
public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] obstaclePrefabs;
    public float spawnInterval = 2f;
    public float spawnZPosition = 30f;

    private float nextSpawnTime;

    private void Update()
    {
        // Check if it is time to spawn the next obstacle
        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    /// <summary>
    /// Spawns a random obstacle in a random lane using the object pool.
    /// </summary>
    private void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0) return;

        // Choose a random lane offset: -1, 0, or 1
        int lane = Random.Range(-1, 2);
        Vector3 spawnPos = new Vector3(lane * 2.5f, 0f, spawnZPosition);

        // Pick a random prefab from the available options
        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        // Get object from pool, position it, and activate
        GameObject obstacle = ObjectPool.Instance.GetObject(prefab);
        obstacle.transform.position = spawnPos;
        obstacle.transform.rotation = Quaternion.identity;
        obstacle.SetActive(true);
    }
}

/// <summary>
/// Simple object pool for reusing obstacle instances.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the ObjectPool.
    /// </summary>
    public static ObjectPool Instance { get; private set; }

    private void Awake() { Instance = this; }

    /// <summary>
    /// Returns a new instance of the given prefab.
    /// </summary>
    public GameObject GetObject(GameObject prefab)
    {
        return Instantiate(prefab);
    }

    /// <summary>
    /// Destroys the object when returned to the pool.
    /// </summary>
    public void ReturnObject(GameObject obj)
    {
        Destroy(obj);
    }
}
