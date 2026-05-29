/// <summary>
/// Manages game state for the dungeon crawler: score, player HP display, game over, and restart.
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
    public TMPro.TextMeshProUGUI hpText;
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
    /// Hides the game over panel on start and updates the UI.
    /// </summary>
    private void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        UpdateUI();
    }

    /// <summary>
    /// Adds points to the score and refreshes the display.
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
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
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
    /// Updates score and player HP text displays.
    /// </summary>
    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        PlayerCrawler player = FindObjectOfType<PlayerCrawler>();
        if (hpText != null && player != null)
            hpText.text = "HP: " + player.health + "/" + player.maxHealth;
    }
}
