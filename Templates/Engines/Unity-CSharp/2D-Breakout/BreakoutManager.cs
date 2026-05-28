using UnityEngine;
using System.Collections.Generic;

public class BreakoutManager : MonoBehaviour
{
    public static BreakoutManager Instance { get; private set; }
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI livesText;
    public GameObject ballPrefab;
    public Transform ballSpawnPoint;
    public Transform paddle;

    private int score;
    private int lives = 3;
    private int totalBlocks;
    private GameObject currentBall;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        totalBlocks = FindObjectsOfType<Block>().Length;
        SpawnBall();
        UpdateUI();
    }

    public void SpawnBall()
    {
        if (currentBall != null) Destroy(currentBall);
        currentBall = Instantiate(ballPrefab, ballSpawnPoint.position, Quaternion.identity);
        Ball ballScript = currentBall.GetComponent<Ball>();
        if (ballScript != null)
            ballScript.Launch();
    }

    public void BlockDestroyed(int points)
    {
        score += points;
        totalBlocks--;
        UpdateUI();

        if (totalBlocks <= 0)
            Debug.Log("Level Complete! Score: " + score);
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

    public void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + score;
        if (livesText != null) livesText.text = "Lives: " + lives;
    }
}
