/// <summary>
/// Manages shoot-em-up game state: score, wave display, game over, and victory panels.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI healthText;
    public TMPro.TextMeshProUGUI waveText;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    private int score;

    /// <summary>
    /// Initializes the singleton instance.
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
    /// Resets score and hides end-game panels.
    /// </summary>
    private void Start()
    {
        score = 0;
        UpdateScoreUI();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    /// <summary>
    /// Adds score points and updates the display.
    /// </summary>
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    /// <summary>
    /// Updates the wave progress text.
    /// </summary>
    public void UpdateWaveText(int wave, int totalWaves)
    {
        if (waveText != null)
            waveText.text = "Wave " + wave + "/" + totalWaves;
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
    /// Shows the victory panel and pauses the game.
    /// </summary>
    public void Victory()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Resumes time and reloads the current scene.
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Updates the score text display.
    /// </summary>
    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}
