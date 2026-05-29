/// <summary>
/// Manages score, high score persistence via PlayerPrefs, game over, and restart for the endless runner.
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
    public TMPro.TextMeshProUGUI highScoreText;
    public GameObject gameOverPanel;

    private int score;
    private int highScore;

    /// <summary>
    /// Initializes the singleton instance and persists across scenes.
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
    /// Resets score, loads saved high score, and hides the game over panel.
    /// </summary>
    private void Start()
    {
        score = 0;
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateUI();
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    /// <summary>
    /// Adds score and updates high score if the current score exceeds it.
    /// </summary>
    public void AddScore(int amount)
    {
        score += amount;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        UpdateUI();
    }

    /// <summary>
    /// Shows the game over panel, saves high score, and pauses the game.
    /// </summary>
    public void GameOver()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Reloads the current scene to restart the game.
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Updates the score and high score text displays.
    /// </summary>
    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (highScoreText != null) highScoreText.text = "High: " + highScore;
    }
}
