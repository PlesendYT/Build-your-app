/// <summary>
/// Enemy wave spawning system with varied enemy types and escalating difficulty.
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveManager : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject basicEnemy;
    public GameObject fastEnemy;
    public GameObject heavyEnemy;
    public GameObject bossEnemy;

    [Header("Spawn Settings")]
    public Transform[] spawnPoints;
    public float spawnDelay = 0.5f;
    public int maxConcurrentEnemies = 30;

    private List<GameObject> activeEnemies = new List<GameObject>();

    public void SpawnWave(int enemyCount)
    {
        StartCoroutine(SpawnWaveCoroutine(enemyCount));
    }

    private IEnumerator SpawnWaveCoroutine(int enemyCount)
    {
        int spawned = 0;
        while (spawned < enemyCount)
        {
            // Clean up destroyed enemies
            activeEnemies.RemoveAll(e => e == null);

            if (activeEnemies.Count < maxConcurrentEnemies)
            {
                // Pick enemy type based on wave progression
                GameObject prefab = ChooseEnemyType(spawned, enemyCount);
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

                GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
                activeEnemies.Add(enemy);

                spawned++;
            }

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private GameObject ChooseEnemyType(int spawnedSoFar, int totalCount)
    {
        float t = (float)spawnedSoFar / totalCount; // 0 to 1 through wave

        // Boss at end of wave
        if (spawnedSoFar == totalCount - 1 && GameManager.Instance.currentWave % 5 == 0)
        {
            return bossEnemy;
        }

        // Weighted enemy selection
        float roll = Random.value;
        if (t > 0.7f && roll < 0.2f) return heavyEnemy;
        if (t > 0.3f && roll < 0.4f) return fastEnemy;
        return basicEnemy;
    }

    public void EnemyDied(GameObject enemy)
    {
        activeEnemies.Remove(enemy);
        int scoreValue = 10;

        // Score based on enemy type
        if (enemy.CompareTag("Heavy")) scoreValue = 30;
        else if (enemy.CompareTag("Fast")) scoreValue = 20;
        else if (enemy.CompareTag("Boss")) scoreValue = 100;

        GameManager.Instance.EnemyKilled(scoreValue);
    }

    public int GetActiveEnemyCount() => activeEnemies.Count;
}
