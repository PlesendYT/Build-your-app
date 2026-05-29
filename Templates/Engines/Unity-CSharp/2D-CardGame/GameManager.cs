/// <summary>
/// Manages the card game turn flow: player turns, opponent turns, health tracking, and game over state.
/// </summary>
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static GameManager Instance { get; private set; }

    public enum GamePhase { Draw, Main, Combat, End }
    public GamePhase currentPhase;

    [Header("Players")]
    public PlayerHand player;
    public PlayerHand opponent;

    [Header("UI")]
    public TMPro.TextMeshProUGUI turnText;
    public TMPro.TextMeshProUGUI playerHealthText;
    public TMPro.TextMeshProUGUI opponentHealthText;
    public GameObject gameOverPanel;

    private int currentTurn;
    private bool isPlayerTurn;

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
    /// Starts the first player turn.
    /// </summary>
    private void Start()
    {
        currentTurn = 1;
        isPlayerTurn = true;
        StartPlayerTurn();
    }

    /// <summary>
    /// Begins the player's turn with draw phase and UI update.
    /// </summary>
    public void StartPlayerTurn()
    {
        currentPhase = GamePhase.Draw;
        player.StartTurn();
        UpdateUI();
    }

    /// <summary>
    /// Ends the player's turn, discards hand, and proceeds to opponent turn.
    /// </summary>
    public void EndPlayerTurn()
    {
        currentPhase = GamePhase.End;
        player.DiscardHand();
        isPlayerTurn = false;
        StartOpponentTurn();
    }

    /// <summary>
    /// Processes the opponent turn (AI stub) and increments turn counter.
    /// </summary>
    private void StartOpponentTurn()
    {
        currentTurn++;
        isPlayerTurn = true;
        StartPlayerTurn();
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
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Updates the turn counter and health display for both players.
    /// </summary>
    private void UpdateUI()
    {
        if (turnText != null) turnText.text = "Turn " + currentTurn;
        if (playerHealthText != null) playerHealthText.text = "HP: " + player.health + "/" + player.maxHealth;
        if (opponentHealthText != null) opponentHealthText.text = "HP: " + opponent.health + "/" + opponent.maxHealth;
    }
}
