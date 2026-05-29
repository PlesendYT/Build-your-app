using UnityEngine;
using UnityEngine.UI;

/// <summary>Manages the player score and updates the UI text component.</summary>
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text? _scoreText;
    
    /// <summary>Current score. Setter is public for reading, private for writing.</summary>
    public int Score { get; private set; }

    /// <summary>Initialises the UI on start.</summary>
    private void Start()
    {
        UpdateUI();
    }

    /// <summary>Adds the specified amount to the score and refreshes the UI.</summary>
    /// <param name="amount">Points to add.</param>
    public void AddScore(int amount)
    {
        Score += amount;
        UpdateUI();
    }

    /// <summary>Updates the on-screen score text.</summary>
    private void UpdateUI()
    {
        if (_scoreText is not null)
            _scoreText.text = $"Score: {Score}";
    }
}
