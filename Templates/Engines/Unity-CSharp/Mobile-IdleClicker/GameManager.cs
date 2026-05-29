using UnityEngine;

/// <summary>
/// Central game manager for the idle clicker. Handles resources, prestige, save/load, and UI.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Singleton instance of the GameManager.
    /// </summary>
    public static GameManager Instance { get; private set; }

    [Header("Resources")]
    public double resource;
    public double totalEarned;
    public double prestigeMultiplier = 1f;

    [Header("UI")]
    public TMPro.TextMeshProUGUI resourceText;
    public TMPro.TextMeshProUGUI incomeText;
    public TMPro.TextMeshProUGUI offlineEarningsText;
    public GameObject offlineEarningsPanel;

    private ResourceGenerator generator;

    private void Awake()
    {
        // Enforce singleton pattern
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        generator = FindAnyObjectByType<ResourceGenerator>();
        LoadGame();
    }

    private void Update()
    {
        UpdateUI();
    }

    /// <summary>
    /// Adds resources to the player's total, tracking lifetime earnings.
    /// </summary>
    public void AddResource(double amount)
    {
        resource += amount;
        totalEarned += amount;
    }

    /// <summary>
    /// Attempts to spend resources. Returns true if the purchase was successful.
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
    /// Checks whether the player can afford a given amount.
    /// </summary>
    public bool CanAfford(double amount)
    {
        return resource >= amount;
    }

    /// <summary>
    /// Displays the offline earnings panel with the earned amount for a few seconds.
    /// </summary>
    public void ShowOfflineEarnings(double amount)
    {
        if (offlineEarningsPanel != null)
        {
            offlineEarningsPanel.SetActive(true);
            if (offlineEarningsText != null)
                offlineEarningsText.text = "Offline Earnings: " + FormatNumber(amount);
            Invoke(nameof(HideOfflinePanel), 3f);
        }
    }

    /// <summary>
    /// Hides the offline earnings panel.
    /// </summary>
    private void HideOfflinePanel()
    {
        if (offlineEarningsPanel != null)
            offlineEarningsPanel.SetActive(false);
    }

    /// <summary>
    /// Performs a prestige reset if the player has earned enough total resources.
    /// </summary>
    public void Prestige()
    {
        if (totalEarned < 10000) return;
        prestigeMultiplier *= 1.5f;
        resource = 0;
        totalEarned = 0;
        SaveGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Saves resource, total earned, and prestige multiplier to PlayerPrefs.
    /// </summary>
    public void SaveGame()
    {
        PlayerPrefs.SetString("Resource", resource.ToString());
        PlayerPrefs.SetString("TotalEarned", totalEarned.ToString());
        PlayerPrefs.SetFloat("PrestigeMultiplier", prestigeMultiplier);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Loads resource, total earned, and prestige multiplier from PlayerPrefs.
    /// </summary>
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("Resource"))
            resource = double.Parse(PlayerPrefs.GetString("Resource"));
        if (PlayerPrefs.HasKey("TotalEarned"))
            totalEarned = double.Parse(PlayerPrefs.GetString("TotalEarned"));
        if (PlayerPrefs.HasKey("PrestigeMultiplier"))
            prestigeMultiplier = PlayerPrefs.GetFloat("PrestigeMultiplier");
    }

    /// <summary>
    /// Refreshes the resource and income display text.
    /// </summary>
    private void UpdateUI()
    {
        if (resourceText != null) resourceText.text = FormatNumber(resource);
        if (incomeText != null && generator != null)
            incomeText.text = "/s: " + FormatNumber(generator.GetTotalProduction());
    }

    /// <summary>
    /// Formats a large number with K, M, B suffixes.
    /// </summary>
    private string FormatNumber(double value)
    {
        if (value >= 1_000_000_000) return (value / 1_000_000_000).ToString("F2") + "B";
        if (value >= 1_000_000) return (value / 1_000_000).ToString("F2") + "M";
        if (value >= 1_000) return (value / 1_000).ToString("F2") + "K";
        return value.ToString("F0");
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveGame();
    }
}
