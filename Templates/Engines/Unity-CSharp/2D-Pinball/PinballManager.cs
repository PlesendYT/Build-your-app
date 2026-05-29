/// <summary>
/// Manages pinball game state: score, lives, ball spawning, and UI updates.
/// </summary>
using UnityEngine;

public class PinballManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static PinballManager Instance { get; private set; }
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI livesText;
    public GameObject ballPrefab;
    public Transform spawnPoint;
    public int lives = 3;

    private int score;
    private GameObject currentBall;

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Spawns the first ball and updates the UI.
    /// </summary>
    private void Start()
    {
        SpawnBall();
        UpdateUI();
    }

    /// <summary>
    /// Destroys the current ball and spawns a new one at the spawn point.
    /// </summary>
    public void SpawnBall()
    {
        if (currentBall != null) Destroy(currentBall);
        currentBall = Instantiate(ballPrefab, spawnPoint.position, Quaternion.identity);
    }

    /// <summary>
    /// Adds points to the score and refreshes the display.
    /// </summary>
    public void AddScore(int points)
    {
        score += points;
        UpdateUI();
    }

    /// <summary>
    /// Called when a ball is lost: decrements lives and spawns a new ball or ends the game.
    /// </summary>
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

    /// <summary>
    /// Updates the score and lives text displays.
    /// </summary>
    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (livesText != null) livesText.text = "Lives: " + lives;
    }

    public int GetScore() => score;
}
