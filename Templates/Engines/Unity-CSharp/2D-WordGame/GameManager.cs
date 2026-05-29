/// <summary>
/// Manages word game state: score, win panel, and scene restart.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static GameManager Instance { get; private set; }
    public TMP_Text scoreText;
    public TMP_Text progressText;
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
    /// Hides the win panel on start.
    /// </summary>
    private void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
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
    /// Shows the win/end panel.
    /// </summary>
    public void GameOver()
    {
        if (winPanel != null) winPanel.SetActive(true);
    }

    /// <summary>
    /// Reloads the current scene.
    /// </summary>
    public void RestartGame()
    {
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
