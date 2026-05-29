/// <summary>
/// Tracks rhythm game scoring with combo multiplier and rating categories (Perfect, Good, Bad, Miss).
/// </summary>
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static ScoreManager Instance { get; private set; }

    [Header("Scoring")]
    private int score;
    private int combo;
    private int maxCombo;
    private int perfectCount;
    private int goodCount;
    private int badCount;
    private int missCount;

    [Header("UI")]
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI comboText;
    public TMPro.TextMeshProUGUI ratingText;

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Adds points based on the rating, updates combo, and applies combo bonus.
    /// </summary>
    public void AddScore(ScoreRating rating)
    {
        int baseScore = rating switch
        {
            ScoreRating.Perfect => 300,
            ScoreRating.Good => 200,
            ScoreRating.Bad => 100,
            ScoreRating.Miss => 0,
            _ => 0
        };

        switch (rating)
        {
            case ScoreRating.Perfect: perfectCount++; combo++; break;
            case ScoreRating.Good: goodCount++; combo++; break;
            case ScoreRating.Bad: badCount++; combo = 0; break;
            case ScoreRating.Miss: missCount++; combo = 0; break;
        }

        maxCombo = Mathf.Max(maxCombo, combo);

        int comboBonus = Mathf.FloorToInt(combo / 10f) * 50;
        score += baseScore + comboBonus;

        UpdateUI();
    }

    /// <summary>
    /// Updates the score and combo text displays.
    /// </summary>
    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = score.ToString("N0");
        if (comboText != null) comboText.text = combo > 1 ? combo + " Combo!" : "";
    }

    public int GetScore() => score;
    public int GetCombo() => combo;
    public int GetMaxCombo() => maxCombo;
}
