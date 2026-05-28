using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Fear System")]
    public int fearLevel;
    public int maxFear = 100;

    [Header("UI")]
    public UnityEngine.UI.Slider fearSlider;
    public UnityEngine.UI.Slider batterySlider;
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    public void AddFear(int amount)
    {
        fearLevel = Mathf.Min(maxFear, fearLevel + amount);
        if (fearLevel >= maxFear) GameOver();
        UpdateUI();
    }

    public void GameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void UpdateUI()
    {
        if (fearSlider != null) fearSlider.value = (float)fearLevel / maxFear;
    }
}
