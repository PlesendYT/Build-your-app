/// <summary>
/// Spawns waves of enemies sequentially, waiting for each wave to be cleared before proceeding.
/// </summary>
using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    /// <summary>
    /// Defines a single wave with enemy type, count, and spawn delays.
    /// </summary>
    [System.Serializable]
    public class Wave
    {
        public GameObject enemyPrefab;
        public int enemyCount;
        public float spawnDelay;
        public float preWaveDelay = 3f;
    }

    [Header("Waves")]
    public Wave[] waves;
    public Transform spawnPoint;
    public bool autoStart = true;

    private int currentWave;
    private bool allWavesComplete;

    /// <summary>
    /// Starts spawning waves automatically if enabled.
    /// </summary>
    private void Start()
    {
        if (autoStart)
            StartCoroutine(SpawnWaves());
    }

    /// <summary>
    /// Spawns each wave after a delay, waiting for all enemies to be cleared between waves.
    /// </summary>
    private IEnumerator SpawnWaves()
    {
        for (int i = 0; i < waves.Length; i++)
        {
            currentWave = i;
            GameManager.Instance.UpdateWaveText(i + 1, waves.Length);

            yield return new WaitForSeconds(waves[i].preWaveDelay);

            for (int j = 0; j < waves[i].enemyCount; j++)
            {
                if (spawnPoint != null)
                {
                    Instantiate(waves[i].enemyPrefab, spawnPoint.position, Quaternion.identity);
                }
                yield return new WaitForSeconds(waves[i].spawnDelay);
            }

            yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0);
        }

        allWavesComplete = true;
        GameManager.Instance.Victory();
    }

    /// <summary>
    /// Resumes the game if paused, allowing the next wave to proceed.
    /// </summary>
    public void StartNextWave()
    {
        if (!allWavesComplete)
            Time.timeScale = 1f;
    }
}
