/// <summary>
/// Manages bullet heaven game state: wave progression, XP collection, player leveling, and score.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Wave Settings")]
    public int currentWave = 1;
    public int enemiesPerWave = 10;
    private int enemiesRemaining;
    public float timeBetweenWaves = 5f;
    private float waveTimer;

    [Header("Player Progression")]
    public int playerLevel = 1;
    public int currentXP;
    public int xpToNextLevel = 100;
    public int score;

    [Header("References")]
    public EnemySpawner enemySpawner;

    /// <summary>
    /// Initializes the first wave.
    /// </summary>
    private void Start()
    {
        enemiesRemaining = enemiesPerWave;
        waveTimer = 2f;
        if (enemySpawner == null)
            enemySpawner = FindObjectOfType<EnemySpawner>();
    }

    /// <summary>
    /// Spawns waves on a timer and tracks remaining enemies.
    /// </summary>
    private void Update()
    {
        if (enemiesRemaining > 0) return;

        waveTimer -= Time.deltaTime;
        if (waveTimer <= 0)
        {
            StartNextWave();
        }
    }

    /// <summary>
    /// Advances to the next wave, increasing difficulty and resetting enemy count.
    /// </summary>
    private void StartNextWave()
    {
        currentWave++;
        enemiesPerWave = Mathf.FloorToInt(enemiesPerWave * 1.3f);
        enemiesRemaining = enemiesPerWave;
        waveTimer = timeBetweenWaves;

        enemySpawner.StartWave(currentWave, enemiesPerWave);
        Debug.Log("Wave " + currentWave + " started with " + enemiesPerWave + " enemies.");
    }

    /// <summary>
    /// Called when an enemy dies — grants XP and updates remaining count.
    /// </summary>
    public void OnEnemyKilled(int xpReward)
    {
        enemiesRemaining--;
        AddXP(xpReward);
        score += 10;
    }

    /// <summary>
    /// Adds XP and handles leveling up.
    /// </summary>
    public void AddXP(int amount)
    {
        currentXP += amount;
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            playerLevel++;
            xpToNextLevel = Mathf.FloorToInt(xpToNextLevel * 1.5f);
            Debug.Log("Level up! Now level " + playerLevel);
        }
    }

    /// <summary>
    /// Returns the XP progress as a 0-1 fraction.
    /// </summary>
    public float GetXPProgress()
    {
        return (float)currentXP / xpToNextLevel;
    }
}
