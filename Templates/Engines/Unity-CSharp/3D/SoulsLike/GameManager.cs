/// <summary>
/// Souls-like game manager: stamina, bonfire respawn, souls currency, and boss encounters.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Player Stats")]
    public int souls;
    public int soulsLostOnDeath;
    public int health = 100;
    public int maxHealth = 100;
    public float stamina = 100f;
    public float maxStamina = 100f;

    [Header("Bonfires")]
    public Transform lastBonfirePosition;
    public int bonfiresLit;

    [Header("Boss")]
    public bool bossAlive = true;
    public string bossName = "Dark Lord";
    public int bossHealth;

    [Header("UI")]
    public TMPro.TextMeshProUGUI soulsText;
    public TMPro.TextMeshProUGUI healthText;
    public TMPro.TextMeshProUGUI staminaText;
    public TMPro.TextMeshProUGUI bossHealthText;
    public GameObject deathPanel;
    public GameObject bossHealthBar;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Update()
    {
        UpdateUI();
    }

    public void AddSouls(int amount)
    {
        souls += amount;
    }

    public void SpendSouls(int amount)
    {
        if (souls >= amount) souls -= amount;
    }

    public void PlayerDied()
    {
        soulsLostOnDeath = Mathf.FloorToInt(souls * 0.3f);
        souls -= soulsLostOnDeath;

        if (deathPanel != null) deathPanel.SetActive(true);
    }

    public void RespawnPlayer()
    {
        // Respawn at last bonfire
        if (lastBonfirePosition != null)
        {
            PlayerController player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                player.transform.position = lastBonfirePosition.position;
                player.RestoreStats();
            }
        }

        if (deathPanel != null) deathPanel.SetActive(false);
    }

    public void LightBonfire(Transform bonfireTransform)
    {
        lastBonfirePosition = bonfireTransform;
        bonfiresLit++;
        health = maxHealth;
        stamina = maxStamina;
    }

    public void BossDefeated()
    {
        bossAlive = false;
        AddSouls(10000);
        if (bossHealthBar != null) bossHealthBar.SetActive(false);
    }

    private void UpdateUI()
    {
        if (soulsText != null) soulsText.text = "Souls: " + souls;
        if (healthText != null) healthText.text = "HP: " + health + "/" + maxHealth;
        if (staminaText != null) staminaText.text = "ST: " + Mathf.FloorToInt(stamina) + "/" + maxStamina;

        if (bossAlive && bossHealthBar != null)
        {
            bossHealthBar.SetActive(true);
            if (bossHealthText != null) bossHealthText.text = bossName + ": " + bossHealth;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
