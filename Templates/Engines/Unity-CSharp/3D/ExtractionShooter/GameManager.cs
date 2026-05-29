/// <summary>
/// Extraction shooter raid manager: timer, loot tracking, PvE combat, and team extraction.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Raid")]
    public float raidDuration = 600f;
    public float remainingTime;
    public bool isRaidActive;

    [Header("Extraction")]
    public int totalExtractionZones;
    public int playersExtracted;

    [Header("UI")]
    public TMPro.TextMeshProUGUI timerText;
    public TMPro.TextMeshProUGUI lootValueText;
    public GameObject extractionSuccessPanel;
    public GameObject deathPanel;

    private int totalLootValue;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        remainingTime = raidDuration;
        isRaidActive = true;
    }

    private void Update()
    {
        if (!isRaidActive) return;
        remainingTime -= Time.deltaTime;
        UpdateUI();

        // Raid ends when timer runs out
        if (remainingTime <= 0f)
        {
            RaidFailed();
        }
    }

    public void AddLootValue(int value)
    {
        totalLootValue += value;
    }

    public void PlayerExtracted()
    {
        playersExtracted++;
        if (playersExtracted >= totalExtractionZones)
        {
            RaidSuccess();
        }
    }

    public void PlayerDied()
    {
        isRaidActive = false;
        if (deathPanel != null) deathPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void RaidSuccess()
    {
        isRaidActive = false;
        if (extractionSuccessPanel != null) extractionSuccessPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void RaidFailed()
    {
        isRaidActive = false;
        if (deathPanel != null) deathPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    private void UpdateUI()
    {
        if (timerText != null)
            timerText.text = Mathf.FloorToInt(remainingTime / 60) + ":" + (remainingTime % 60).ToString("00");
        if (lootValueText != null)
            lootValueText.text = "Loot: $" + totalLootValue;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
