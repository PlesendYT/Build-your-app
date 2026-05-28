using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] obstaclePrefabs;
    public float spawnInterval = 2f;
    public float spawnZPosition = 30f;

    private float nextSpawnTime;

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnObstacle();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

    private void SpawnObstacle()
    {
        if (obstaclePrefabs.Length == 0) return;

        int lane = Random.Range(-1, 2);
        Vector3 spawnPos = new Vector3(lane * 2.5f, 0f, spawnZPosition);

        GameObject prefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];

        GameObject obstacle = ObjectPool.Instance.GetObject(prefab);
        obstacle.transform.position = spawnPos;
        obstacle.transform.rotation = Quaternion.identity;
        obstacle.SetActive(true);
    }
}

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance { get; private set; }

    private void Awake() { Instance = this; }

    public GameObject GetObject(GameObject prefab)
    {
        return Instantiate(prefab);
    }

    public void ReturnObject(GameObject obj)
    {
        Destroy(obj);
    }
}
