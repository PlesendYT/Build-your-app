/// <summary>
/// Manages rhythm game state: starts the song, handles song end and game over panels.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static GameManager Instance { get; private set; }

    [Header("References")]
    public NoteSpawner noteSpawner;
    public ScoreManager scoreManager;

    [Header("UI")]
    public GameObject songEndPanel;
    public GameObject gameOverPanel;

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// Starts the song playback on game start.
    /// </summary>
    private void Start()
    {
        if (noteSpawner != null)
            noteSpawner.StartSong();
    }

    /// <summary>
    /// Shows the song end panel when the song finishes.
    /// </summary>
    public void SongEnded()
    {
        if (songEndPanel != null)
            songEndPanel.SetActive(true);
    }

    /// <summary>
    /// Shows the game over panel.
    /// </summary>
    public void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    /// <summary>
    /// Reloads the current scene.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
