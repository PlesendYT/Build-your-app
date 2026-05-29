/// <summary>
/// Manages metroidvania game state: keys, power-ups, save points, area transitions, and game over.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for global access and persistence across scenes.
    /// </summary>
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    public GameObject gameOverPanel;
    public TMPro.TextMeshProUGUI healthText;

    [Header("Game State")]
    public List<string> collectedKeys = new List<string>();
    public List<PowerUpType> unlockedPowerUps = new List<PowerUpType>();
    public Vector2 lastSavePoint;

    /// <summary>
    /// Initializes the singleton and persists across scenes.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Adds a key to the collection.
    /// </summary>
    public void AddKey(string keyId)
    {
        if (!collectedKeys.Contains(keyId))
            collectedKeys.Add(keyId);
    }

    /// <summary>
    /// Checks if a specific key has been collected.
    /// </summary>
    public bool HasKey(string keyId)
    {
        return collectedKeys.Contains(keyId);
    }

    /// <summary>
    /// Saves the player's position at a save point.
    /// </summary>
    public void SaveAtPoint(Vector2 point)
    {
        lastSavePoint = point;
    }

    /// <summary>
    /// Shows the game over panel and pauses the game.
    /// </summary>
    public void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Restarts the current scene from the last save point.
    /// </summary>
    public void RestartFromSave()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Loads the next area (next scene in build index).
    /// </summary>
    public void LoadNextArea()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
    }
}
