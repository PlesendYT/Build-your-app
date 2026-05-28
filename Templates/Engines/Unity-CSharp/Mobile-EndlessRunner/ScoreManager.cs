using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
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
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        UpdateUI();
    }

    private void Update()
    {
        score += scorePerSecond * scoreMultiplier * Time.deltaTime;
        int displayScore = Mathf.FloorToInt(score);
        if (displayScore > highScore)
        {
            highScore = displayScore;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        UpdateUI();
    }

    public void AddBonus(int amount)
    {
        score += amount;
    }

    public int GetScore() => Mathf.FloorToInt(score);
    public int GetHighScore() => highScore;

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = GetScore().ToString("N0");
        if (highScoreText != null) highScoreText.text = "Best: " + highScore.ToString("N0");
    }
}
