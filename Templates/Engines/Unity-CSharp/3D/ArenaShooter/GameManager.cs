/// <summary>
/// Arena shooter manager: wave combat, score tracking, power-ups, and survival progression.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Arena")]
    public int currentWave = 1, score, kills, lives = 3;
    public int baseEnemies = 5, enemiesRemaining;
    public float timeBetweenWaves = 5f;
    public bool isWaveActive, isBetweenWaves;

    [Header("UI")]
    public TMPro.TextMeshProUGUI waveText, scoreText, livesText, enemiesText;
    public GameObject gameOverPanel, waveCompletePanel;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Start() => StartNextWave();

    private void Update()
    {
        if (waveText != null) waveText.text = "Wave " + currentWave;
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (livesText != null) livesText.text = "Lives: " + lives;
        if (enemiesText != null) enemiesText.text = "Enemies: " + enemiesRemaining;
    }

    public void StartNextWave()
    {
        currentWave++; enemiesRemaining = baseEnemies + (currentWave * 2); isWaveActive = true; isBetweenWaves = false;
        if (waveCompletePanel != null) waveCompletePanel.SetActive(false);
        WaveManager wm = FindObjectOfType<WaveManager>();
        if (wm != null) wm.SpawnWave(enemiesRemaining);
    }

    public void EnemyKilled(int val) { kills++; score += val * currentWave; enemiesRemaining--; if (enemiesRemaining <= 0) { isWaveActive = false; isBetweenWaves = true; if (waveCompletePanel != null) waveCompletePanel.SetActive(true); Invoke(nameof(StartNextWave), timeBetweenWaves); } }

    public void PlayerDied()
    {
        lives--;
        if (lives <= 0) { if (gameOverPanel != null) gameOverPanel.SetActive(true); Time.timeScale = 0f; }
        else { PlayerController p = FindObjectOfType<PlayerController>(); if (p != null) p.Respawn(); }
    }

    public void RestartGame() { Time.timeScale = 1f; SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); }
}
