/// <summary>
/// Manages hidden object game state: level completion, messages, and scene navigation.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static GameManager Instance { get; private set; }
    public TMPro.TextMeshProUGUI messageText;
    public GameObject levelCompletePanel;
    public float messageDuration = 2f;

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Hides the level complete panel on start.
    /// </summary>
    private void Start()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
    }

    /// <summary>
    /// Shows the level complete panel and a completion message.
    /// </summary>
    public void LevelComplete()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(true);
        ShowMessage("Level Complete!");
    }

    /// <summary>
    /// Displays a temporary message that clears after the specified duration.
    /// </summary>
    public void ShowMessage(string msg)
    {
        if (messageText != null)
        {
            messageText.text = msg;
            Invoke(nameof(ClearMessage), messageDuration);
        }
    }

    /// <summary>
    /// Clears the message text.
    /// </summary>
    private void ClearMessage()
    {
        if (messageText != null)
            messageText.text = "";
    }

    /// <summary>
    /// Loads the next level (placeholder: reloads current scene).
    /// </summary>
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Returns to the main menu (scene index 0).
    /// </summary>
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
