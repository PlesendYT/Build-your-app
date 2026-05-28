using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text? _scoreText;
    
    public int Score { get; private set; }

    private void Start()
    {
        UpdateUI();
    }

    public void AddScore(int amount)
    {
        Score += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_scoreText is not null)
            _scoreText.text = $"Score: {Score}";
    }
}
