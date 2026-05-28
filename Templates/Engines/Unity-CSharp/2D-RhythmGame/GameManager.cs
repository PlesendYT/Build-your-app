using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("References")]
    public NoteSpawner noteSpawner;
    public ScoreManager scoreManager;

    [Header("UI")]
    public GameObject songEndPanel;
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        if (noteSpawner != null)
            noteSpawner.StartSong();
    }

    public void SongEnded()
    {
        if (songEndPanel != null)
            songEndPanel.SetActive(true);
    }

    public void GameOver()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
