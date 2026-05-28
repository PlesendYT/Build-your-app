using UnityEngine;

public class PinballManager : MonoBehaviour
{
    public static PinballManager Instance { get; private set; }
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI livesText;
    public GameObject ballPrefab;
    public Transform spawnPoint;
    public int lives = 3;

    private int score;
    private GameObject currentBall;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        SpawnBall();
        UpdateUI();
    }

    public void SpawnBall()
    {
        if (currentBall != null) Destroy(currentBall);
        currentBall = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }

    public void BallLost()
    {
        lives--;
        UpdateUI();
        if (lives <= 0)
        {
            Debug.Log("Game Over! Score: " + score);
        }
        else
        {
            SpawnBall();
        }
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (livesText != null) livesText.text = "Lives: " + lives;
    }

    public int GetScore() => score;
}
