/// <summary>
/// Manages minesweeper game state: win/lose panels, messages, and restart.
/// </summary>
/// <summary>
/// Manages Minesweeper game state: win/loss panels, UI updates, and scene restart.
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
    public TMPro.TextMeshProUGUI mineCountText;
    public TMPro.TextMeshProUGUI gameStateText;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

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
    /// Hides end-game panels on start.
    /// </summary>
    private void Start()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
    }

    /// <summary>
    /// Shows the appropriate panel based on win/loss and pauses the game.
    /// </summary>
    public void GameOver(bool won)
    {
        if (won)
        {
            if (victoryPanel != null)
                victoryPanel.SetActive(true);
        }
        else
        {
            if (gameOverPanel != null)
                gameOverPanel.SetActive(true);
        }
        Time.timeScale = 0f;
        UpdateUI();
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
    /// Updates the game state text to indicate game over.
    /// </summary>
    private void UpdateUI()
    {
        if (gameStateText != null)
            gameStateText.text = "Game Over!";
    }
}

    /// <summary>
    /// Initializes the board and hides win/lose panels on start.
    /// </summary>
    private void Start()
    {
        if (board != null) board.Init();
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

    /// <summary>
    /// Shows the win or lose panel and a corresponding message.
    /// </summary>
    public void GameOver(bool won)
    {
        if (won)
        {
            if (winPanel != null) winPanel.SetActive(true);
            ShowMessage("You Win!");
        }
        else
        {
            if (losePanel != null) losePanel.SetActive(true);
            ShowMessage("Game Over!");
        }
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
}
