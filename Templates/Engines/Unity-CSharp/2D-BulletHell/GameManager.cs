/// <summary>
/// Manages score with chain multiplier, game over state, and scene restart for the bullet hell game.
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
    public TMPro.TextMeshProUGUI livesText;
    public GameObject gameOverPanel;
    private int score;
    private int chainMultiplier = 1;

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
    /// Resets score and hides the game over panel on start.
    /// </summary>
    private void Start()
    {
        score = 0;
        UpdateUI();
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    /// <summary>
    /// Adds score multiplied by the chain multiplier and increments the chain.
    /// </summary>
    public void AddScore(int basePoints)
    {
        score += basePoints * chainMultiplier;
        chainMultiplier = Mathf.Min(chainMultiplier + 1, 10);
        UpdateUI();
    }

    /// <summary>
    /// Resets the chain multiplier back to 1 (e.g., when the player is hit).
    /// </summary>
    public void BreakChain()
    {
        chainMultiplier = 1;
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
    /// Reloads the current scene to restart the game.
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Updates the score and chain multiplier display.
    /// </summary>
    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score + " x" + chainMultiplier;
    }
}
