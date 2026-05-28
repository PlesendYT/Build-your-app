using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    private void Start()
    {
        currentTurn = 1;
        isPlayerTurn = true;
        StartPlayerTurn();
    }

    public void StartPlayerTurn()
    {
        currentPhase = GamePhase.Draw;
        player.StartTurn();
        UpdateUI();
    }

    public void EndPlayerTurn()
    {
        currentPhase = GamePhase.End;
        player.DiscardHand();
        isPlayerTurn = false;
        StartOpponentTurn();
    }

    private void StartOpponentTurn()
    {
        // AI opponent logic here
        currentTurn++;
        isPlayerTurn = true;
        StartPlayerTurn();
    }

    public void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    private void UpdateUI()
    {
        if (turnText != null) turnText.text = "Turn " + currentTurn;
        if (playerHealthText != null) playerHealthText.text = "HP: " + player.health + "/" + player.maxHealth;
        if (opponentHealthText != null) opponentHealthText.text = "HP: " + opponent.health + "/" + opponent.maxHealth;
    }
}
