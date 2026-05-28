using UnityEngine;

public class GameManager : MonoBehaviour
{
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

    public void AddResource(double amount)
    {
        resource += amount;
        totalEarned += amount;
    }

    public bool SpendResource(double amount)
    {
        if (resource >= amount)
        {
            resource -= amount;
            return true;
        }
        return false;
    }

    public bool CanAfford(double amount)
    {
        return resource >= amount;
    }

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

    private void HideOfflinePanel()
    {
        if (offlineEarningsPanel != null)
            offlineEarningsPanel.SetActive(false);
    }

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

    public void SaveGame()
    {
        PlayerPrefs.SetString("Resource", resource.ToString());
        PlayerPrefs.SetString("TotalEarned", totalEarned.ToString());
        PlayerPrefs.SetFloat("PrestigeMultiplier", prestigeMultiplier);
        PlayerPrefs.Save();
    }

    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("Resource"))
            resource = double.Parse(PlayerPrefs.GetString("Resource"));
        if (PlayerPrefs.HasKey("TotalEarned"))
            totalEarned = double.Parse(PlayerPrefs.GetString("TotalEarned"));
        if (PlayerPrefs.HasKey("PrestigeMultiplier"))
            prestigeMultiplier = PlayerPrefs.GetFloat("PrestigeMultiplier");
    }

    private void UpdateUI()
    {
        if (resourceText != null) resourceText.text = FormatNumber(resource);
        if (incomeText != null && generator != null)
            incomeText.text = "/s: " + FormatNumber(generator.GetTotalProduction());
    }

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
