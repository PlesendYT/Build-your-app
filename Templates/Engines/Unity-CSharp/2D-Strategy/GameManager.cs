/// <summary>
/// Manages turn-based strategy game state: gold, income per turn, victory/defeat conditions.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static GameManager Instance { get; private set; }

    [Header("References")]
    public TurnManager turnManager;

    [Header("Resources")]
    public int gold = 100;
    public int incomePerTurn = 10;

    [Header("UI")]
    public TMPro.TextMeshProUGUI goldText;
    public TMPro.TextMeshProUGUI turnText;
    public GameObject victoryPanel;
    public GameObject gameOverPanel;

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
    }

    /// <summary>
    /// Adds income per turn and updates the UI.
    /// </summary>
    public void NextTurn()
    {
        gold += incomePerTurn;
        UpdateUI();
    }

    /// <summary>
    /// Attempts to spend gold, returns false if insufficient.
    /// </summary>
    public bool SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            UpdateUI();
            return true;
        }
        return false;
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
    /// Updates the gold and turn number displays.
    /// </summary>
    public void UpdateUI()
    {
        if (goldText != null) goldText.text = "Gold: " + gold;
        if (turnText != null && turnManager != null)
            turnText.text = "Turn: " + turnManager.currentTurn;
    }
}
