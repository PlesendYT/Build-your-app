/// <summary>
/// Manages Tetris game state: score, level progression, game over, and scene restart.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static GameManager Instance { get; private set; }
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI levelText;
    public GameObject gameOverPanel;
    private int score;
    private int level = 1;
    private int linesCleared;

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Hides the game over panel on start.
    /// </summary>
    private void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        UpdateUI();
    }

    /// <summary>
    /// Adds score, tracks lines cleared, and levels up every 10 lines per level.
    /// </summary>
    public void AddScore(int points)
    {
        score += points;
        linesCleared++;
        if (linesCleared >= level * 10)
        {
            level++;
            linesCleared = 0;
        }
        UpdateUI();
    }

    /// <summary>
    /// Shows the game over panel and pauses the game.
    /// </summary>
    public void GameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
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
    /// Updates the score and level text displays.
    /// </summary>
    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (levelText != null) levelText.text = "Level: " + level;
    }
}
