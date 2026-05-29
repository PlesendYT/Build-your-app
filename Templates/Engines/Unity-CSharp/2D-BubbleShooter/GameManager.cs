/// <summary>
/// Manages game-wide state for Bubble Shooter: score tracking, level completion, and scene restart.
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
    public GameObject winPanel;
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
    /// Hides the win panel on start and refreshes the UI.
    /// </summary>
    private void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
        UpdateUI();
    }

    /// <summary>
    /// Adds points to the score and updates the display.
    /// </summary>
    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }

    /// <summary>
    /// Shows the level completion panel.
    /// </summary>
    public void LevelComplete()
    {
        if (winPanel != null) winPanel.SetActive(true);
    }

    /// <summary>
    /// Reloads the current scene to restart the game.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Updates the score text UI element.
    /// </summary>
    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
    }
}
