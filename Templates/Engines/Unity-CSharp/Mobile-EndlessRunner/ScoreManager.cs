using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages scoring, high score persistence, and UI updates for the endless runner.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the ScoreManager.
    /// </summary>
    public static ScoreManager Instance { get; private set; }

    [Header("Scoring")]
    public float score;
    public float scoreMultiplier = 1f;
    public float scorePerSecond = 10f;

    [Header("UI")]
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI highScoreText;

    private int highScore;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        // Load saved high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateUI();
    }

    private void Update()
    {
        // Accumulate score over time based on multiplier
        score += scorePerSecond * scoreMultiplier * Time.deltaTime;
        int displayScore = Mathf.FloorToInt(score);

        // Update high score if current score exceeds it
        if (displayScore > highScore)
        {
            highScore = displayScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        UpdateUI();
    }

    /// <summary>
    /// Adds a one-time bonus to the score.
    /// </summary>
    public void AddBonus(int amount)
    {
        score += amount;
    }

    /// <summary>
    /// Returns the current score as an integer.
    /// </summary>
    public int GetScore() => Mathf.FloorToInt(score);

    /// <summary>
    /// Returns the high score.
    /// </summary>
    public int GetHighScore() => highScore;

    /// <summary>
    /// Refreshes the score and high score UI text.
    /// </summary>
    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = GetScore().ToString("N0");
        if (highScoreText != null) highScoreText.text = "Best: " + highScore.ToString("N0");
    }
}
