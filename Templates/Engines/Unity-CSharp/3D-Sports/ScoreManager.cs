/// <summary>
/// Tracks scores and triggers match end; includes GameManager for restart logic.
/// </summary>
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int playerScore;
    public int opponentScore;
    public int scoreToWin = 5;

    [Header("UI")]
    public TMPro.TextMeshProUGUI scoreText;

    private void Awake() { Instance = this; }

    public void Score(int team)
    {
        if (team == 0) playerScore++;
        else opponentScore++;

        if (playerScore >= scoreToWin || opponentScore >= scoreToWin)
            GameManager.Instance.MatchOver();

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null)
            scoreText.text = playerScore + " - " + opponentScore;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public GameObject matchOverPanel;

    private void Awake() { Instance = this; }

    public void MatchOver()
    {
        if (matchOverPanel != null) matchOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
