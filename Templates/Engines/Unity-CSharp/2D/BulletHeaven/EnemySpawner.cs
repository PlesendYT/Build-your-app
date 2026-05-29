/// <summary>
/// Handles wave-based enemy spawning for the bullet heaven game mode.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public float spawnInterval = 0.5f;

    [Header("Current Wave")]
    private int currentWave;
    private int enemiesToSpawn;
    private int enemiesSpawned;
    private float spawnTimer;
    private bool isSpawning;

    [Header("References")]
    public GameManager gameManager;

    /// <summary>
    /// Caches the game manager reference.
    /// </summary>
    private void Start()
    {
        if (gameManager == null)
            gameManager = FindObjectOfType<GameManager>();
    }

    /// <summary>
    /// Spawns enemies at intervals during an active wave.
    /// </summary>
    private void Update()
    {
        if (!isSpawning || enemiesSpawned >= enemiesToSpawn) return;

        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            SpawnEnemy();
            spawnTimer = spawnInterval;
        }
    }

    /// <summary>
    /// Begins a new wave with the specified number of enemies.
    /// </summary>
    public void StartWave(int waveNumber, int enemyCount)
    {
        currentWave = waveNumber;
        enemiesToSpawn = enemyCount;
        enemiesSpawned = 0;
        isSpawning = true;
        spawnTimer = 0f;
    }

    /// <summary>
    /// Instantiates a random enemy at a random spawn point around the map.
    /// </summary>
    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length == 0 || spawnPoints.Length == 0) return;

        // Pick a random enemy type and spawn point
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        Enemy enemyScript = enemy.GetComponent<Enemy>();

        // Scale enemy stats with the current wave
        if (enemyScript != null)
        {
            enemyScript.health = Mathf.FloorToInt(enemyScript.health * (1f + currentWave * 0.1f));
            enemyScript.damage = Mathf.FloorToInt(enemyScript.damage * (1f + currentWave * 0.05f));
            enemyScript.xpReward = Mathf.FloorToInt(enemyScript.xpReward * (1f + currentWave * 0.15f));
        }

        enemiesSpawned++;
    }

    /// <summary>
    /// Stops the current wave from spawning more enemies.
    /// </summary>
    public void StopSpawning()
    {
        isSpawning = false;
    }
}
