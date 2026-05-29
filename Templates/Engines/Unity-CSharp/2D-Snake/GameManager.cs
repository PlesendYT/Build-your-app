/// <summary>
/// Manages Snake game state: score tracking, game over panel, and scene restart.
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
    public GameObject gameOverPanel;
    private int score;

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
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        UpdateUI();
    }

    /// <summary>
    /// Adds score points and updates the display.
    /// </summary>
    public void AddScore(int points)
    {
        score += points;
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
    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }
}
