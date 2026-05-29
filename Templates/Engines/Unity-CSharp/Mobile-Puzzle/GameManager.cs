using UnityEngine;

/// <summary>
/// Central game manager for the mobile puzzle game. Handles menu navigation and level loading.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the GameManager.
    /// </summary>
    public static GameManager Instance { get; private set; }

    [Header("UI")]
    public GameObject mainMenuPanel;
    public GameObject levelSelectPanel;

    private void Awake()
    {
        // Enforce singleton pattern
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    /// <summary>
    /// Shows the level select panel and hides the main menu.
    /// </summary>
    public void ShowLevelSelect()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (levelSelectPanel != null) levelSelectPanel.SetActive(true);
    }

    /// <summary>
    /// Shows the main menu panel and hides the level select.
    /// </summary>
    public void ShowMainMenu()
    {
        if (levelSelectPanel != null) levelSelectPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    /// <summary>
    /// Loads the scene corresponding to the given level index.
    /// </summary>
    public void LoadLevel(int levelIndex)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex);
    }

    /// <summary>
    /// Quits the application.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
