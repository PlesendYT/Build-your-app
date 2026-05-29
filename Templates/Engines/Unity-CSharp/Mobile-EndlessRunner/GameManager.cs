using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Core game manager for the endless runner. Controls game states, UI panels, and time scale.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the GameManager.
    /// </summary>
    public static GameManager Instance { get; private set; }

    [Header("Settings")]
    public bool isGameOver;

    [Header("UI")]
    public GameObject gameOverPanel;
    public GameObject startPanel;
    public GameObject pauseButton;

    private void Awake()
    {
        // Enforce singleton pattern
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        // Show start panel and pause the game initially
        if (startPanel != null) startPanel.SetActive(true);
        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Starts the game by hiding the start panel, resetting game over state, and resuming time.
    /// </summary>
    public void StartGame()
    {
        if (startPanel != null) startPanel.SetActive(false);
        if (pauseButton != null) pauseButton.SetActive(true);
        isGameOver = false;
        Time.timeScale = 1f;
    }

    /// <summary>
    /// Triggers game over: shows the game over panel and freezes time.
    /// </summary>
    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    /// <summary>
    /// Reloads the current scene to restart the game.
    /// </summary>
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Toggles the game between paused and running states.
    /// </summary>
    public void Pause()
    {
        Time.timeScale = Time.timeScale == 0f ? 1f : 0f;
    }
}
