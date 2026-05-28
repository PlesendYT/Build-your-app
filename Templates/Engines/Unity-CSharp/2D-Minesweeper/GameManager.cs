using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TMPro.TextMeshProUGUI messageText;
    public GameObject winPanel;
    public GameObject losePanel;
    public MinesweeperBoard board;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (board != null) board.Init();
        if (winPanel != null) winPanel.SetActive(false);
        if (losePanel != null) losePanel.SetActive(false);
    }

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

    public void ShowMessage(string msg)
    {
        if (messageText != null)
            messageText.text = msg;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
