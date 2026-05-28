using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public TMPro.TextMeshProUGUI messageText;
    public GameObject levelCompletePanel;
    public float messageDuration = 2f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
    }

    public void LevelComplete()
    {
        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(true);
        ShowMessage("Level Complete!");
    }

    public void ShowMessage(string msg)
    {
        if (messageText != null)
        {
            messageText.text = msg;
            Invoke(nameof(ClearMessage), messageDuration);
        }
    }

    private void ClearMessage()
    {
        if (messageText != null)
            messageText.text = "";
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
