/// <summary>
/// Manages memory game state: score, win condition, messages, and restart.
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
    public TMPro.TextMeshProUGUI messageText;
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
    /// Hides the win panel on start and updates UI.
    /// </summary>
    private void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
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
    /// Shows the win panel with a victory message.
    /// </summary>
    public void GameOver()
    {
        if (winPanel != null) winPanel.SetActive(true);
        ShowMessage("All pairs found! Score: " + score);
    }

    /// <summary>
    /// Displays a message on the UI.
    /// </summary>
    public void ShowMessage(string msg)
    {
        if (messageText != null)
            messageText.text = msg;
    }

    /// <summary>
    /// Reloads the current scene to restart the game.
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
