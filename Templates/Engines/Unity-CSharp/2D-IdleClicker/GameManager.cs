/// <summary>
/// Manages idle clicker resources, save/load, UI formatting, and provides singleton access.
/// </summary>
using UnityEngine;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static GameManager Instance { get; private set; }

    [Header("Resources")]
    public double resource;
    public double totalEarned;

    [Header("UI")]
    public TMPro.TextMeshProUGUI resourceText;
    public TMPro.TextMeshProUGUI incomeText;

    private ResourceGenerator generator;

    /// <summary>
    /// Initializes the singleton instance and persists across scenes.
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
    /// Finds the resource generator and loads saved game data.
    /// </summary>
    private void Start()
    {
        generator = FindAnyObjectByType<ResourceGenerator>();
        LoadGame();
    }

    /// <summary>
    /// Updates the UI each frame.
    /// </summary>
    private void Update()
    {
        UpdateUI();
    }

    /// <summary>
    /// Adds resources and tracks total earned.
    /// </summary>
    public void AddResource(double amount)
    {
        resource += amount;
        totalEarned += amount;
    }

    /// <summary>
    /// Spends resources if enough are available; returns true on success.
    /// </summary>
    public bool SpendResource(double amount)
    {
        if (resource >= amount)
        {
            resource -= amount;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Checks if the player can afford a given amount.
    /// </summary>
    public bool CanAfford(double amount)
    {
        return resource >= amount;
    }

    /// <summary>
    /// Saves resources to PlayerPrefs.
    /// </summary>
    public void SaveGame()
    {
        PlayerPrefs.SetString("Resource", resource.ToString());
        PlayerPrefs.SetString("TotalEarned", totalEarned.ToString());
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads saved resources from PlayerPrefs.
    /// </summary>
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("Resource"))
            resource = double.Parse(PlayerPrefs.GetString("Resource"));
        if (PlayerPrefs.HasKey("TotalEarned"))
            totalEarned = double.Parse(PlayerPrefs.GetString("TotalEarned"));
    }

    /// <summary>
    /// Resets all progress and reloads the scene.
    /// </summary>
    public void ResetGame()
    {
        resource = 0;
        totalEarned = 0;
        SaveGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Updates resource and income per second text displays with formatted numbers.
    /// </summary>
    private void UpdateUI()
    {
        if (resourceText != null)
            resourceText.text = FormatNumber(resource);
        if (incomeText != null && generator != null)
            incomeText.text = "per second: " + FormatNumber(generator.GetTotalProductionPerSecond());
    }

    /// <summary>
    /// Formats large numbers with K/M/B suffixes.
    /// </summary>
    private string FormatNumber(double value)
    {
        if (value >= 1_000_000_000) return (value / 1_000_000_000).ToString("F2") + "B";
        if (value >= 1_000_000) return (value / 1_000_000).ToString("F2") + "M";
        if (value >= 1_000) return (value / 1_000).ToString("F2") + "K";
        return value.ToString("F0");
    }

    /// <summary>
    /// Auto-saves when the application quits.
    /// </summary>
    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
