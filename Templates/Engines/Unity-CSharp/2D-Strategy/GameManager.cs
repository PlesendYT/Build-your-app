using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
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

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void NextTurn()
    {
        gold += incomePerTurn;
        UpdateUI();
    }

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

    public void Victory()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
        Time.timeScale = 0f;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateUI()
    {
        if (goldText != null) goldText.text = "Gold: " + gold;
        if (turnText != null && turnManager != null)
            turnText.text = "Turn: " + turnManager.currentTurn;
    }
}
