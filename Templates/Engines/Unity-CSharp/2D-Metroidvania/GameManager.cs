using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    public GameObject gameOverPanel;
    public TMPro.TextMeshProUGUI healthText;

    [Header("Game State")]
    public List<string> collectedKeys = new List<string>();
    public List<PowerUpType> unlockedPowerUps = new List<PowerUpType>();
    public Vector2 lastSavePoint;

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

    public void AddKey(string keyId)
    {
        if (!collectedKeys.Contains(keyId))
            collectedKeys.Add(keyId);
    }

    public bool HasKey(string keyId)
    {
        return collectedKeys.Contains(keyId);
    }

    public void SaveAtPoint(Vector2 point)
    {
        lastSavePoint = point;
    }

    public void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void RestartFromSave()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextArea()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex < SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(nextIndex);
    }
}
