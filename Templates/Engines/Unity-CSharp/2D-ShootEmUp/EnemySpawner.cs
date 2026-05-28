using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public GameObject[] enemyPrefabs;
        public int enemyCount;
        public float spawnDelay = 0.5f;
        public float preWaveDelay = 2f;
    }

    [Header("Spawn Settings")]
    public Wave[] waves;
    public Vector2 spawnArea = new Vector2(7f, 1f);
    public float spawnHeight = 6f;
    public int currentWave;

    [Header("Boss")]
    public GameObject bossPrefab;
    public int bossWave = 3;

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            currentWave = i;
            GameManager.Instance?.UpdateWaveText(i + 1, waves.Length);

            yield return new WaitForSeconds(waves[i].preWaveDelay);

            if (i == bossWave && bossPrefab != null)
            {
                Instantiate(bossPrefab, new Vector3(0, spawnHeight, 0), Quaternion.identity);
            }
            else
            {
                for (int j = 0; j < waves[i].enemyCount; j++)
                {
                    SpawnEnemy(waves[i]);
                    yield return new WaitForSeconds(waves[i].spawnDelay);
                }
            }

            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
        }

        GameManager.Instance?.Victory();
    }

    private void SpawnEnemy(Wave wave)
    {
        if (wave.enemyPrefabs.Length == 0) return;

        GameObject prefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];
        Vector3 position = new Vector3(
            Random.Range(-spawnArea.x, spawnArea.x),
            spawnHeight,
            0f
        );
        Instantiate(prefab, position, Quaternion.identity);
    }
}
