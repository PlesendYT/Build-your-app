/// <summary>
/// Manages game-wide state including scoring, wave tracking, game over, and scene restart.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static GameManager Instance { get; private set; }

    [Header("Wave System")]
    public EnemySpawner spawner;
    public int currentWave;
    public int totalWaves;

    [Header("UI")]
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI waveText;
    public GameObject gameOverPanel;

    private int score;

    /// <summary>
    /// Initializes the singleton instance and persists it across scenes.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Adds points to the score and refreshes the UI.
    /// </summary>
    public void AddScore(int amount)
    {
        score += amount;
        UpdateUI();
    }

    /// <summary>
    /// Shows the game over panel and pauses the game.
    /// </summary>
    public void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Restarts the current scene and resumes time.
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Updates the score and wave display text.
    /// </summary>
    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (waveText != null) waveText.text = "Wave: " + currentWave + "/" + totalWaves;
    }
}
