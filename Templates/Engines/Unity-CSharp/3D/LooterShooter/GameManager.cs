/// <summary>
/// Looter shooter game manager handling loot tiers, enemy drops, and session progression.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Loot Settings")]
    public AnimationCurve lootTierCurve;
    public string[] lootTierNames = { "Common", "Uncommon", "Rare", "Epic", "Legendary" };
    public Color[] lootTierColors;

    [Header("Session")]
    public int playerLevel = 1;
    public int totalExp;
    public int kills;

    [Header("UI")]
    public TMPro.TextMeshProUGUI levelText;
    public TMPro.TextMeshProUGUI killText;
    public GameObject gameOverPanel;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Update()
    {
        UpdateUI();
    }

    public int RollLootTier()
    {
        // Weighted random based on player level
        float roll = Random.value;
        float tierChance = 0.6f - (playerLevel * 0.01f); // Better loot at higher levels
        tierChance = Mathf.Clamp(tierChance, 0.2f, 0.8f);

        if (roll < tierChance) return 0; // Common
        if (roll < tierChance + 0.25f) return 1; // Uncommon
        if (roll < tierChance + 0.40f) return 2; // Rare
        if (roll < tierChance + 0.48f) return 3; // Epic
        return 4; // Legendary
    }

    public void AddExperience(int exp)
    {
        totalExp += exp;
        int newLevel = 1 + Mathf.FloorToInt(totalExp / 100f);
        if (newLevel > playerLevel)
        {
            playerLevel = newLevel;
        }
    }

    public void RegisterKill()
    {
        kills++;
    }

    public void PlayerDied()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void UpdateUI()
    {
        if (levelText != null) levelText.text = "Level: " + playerLevel;
        if (killText != null) killText.text = "Kills: " + kills;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
