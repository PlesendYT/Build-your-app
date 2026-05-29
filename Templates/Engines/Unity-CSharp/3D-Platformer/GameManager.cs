/// <summary>
/// 3D platformer manager: score tracking, game-over panel, and scene restart.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI healthText;
    public GameObject gameOverPanel;
    private int score;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (scoreText != null) scoreText.text = "Score: " + score;
    }

    public void GameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
