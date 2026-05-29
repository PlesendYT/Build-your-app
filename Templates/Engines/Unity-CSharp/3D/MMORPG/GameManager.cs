/// <summary>
/// MMORPG manager: player persistence, character stats, quest tracking, and world state.
/// </summary>
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

[System.Serializable]
public class PlayerSaveData
{
    public string playerName = "Hero";
    public int level = 1, exp, expToNext = 100;
    public int health = 100, maxHealth = 100, mana = 50, maxMana = 50, gold;
    public List<string> completedQuests = new List<string>();
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public PlayerSaveData playerData = new PlayerSaveData();
    public string currentZone = "Starter Village";

    [Header("UI")]
    public TMPro.TextMeshProUGUI levelText, healthText, manaText, goldText, zoneText;

    private void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(gameObject); }
    }

    private void Update()
    {
        if (levelText != null) levelText.text = "Lv." + playerData.level + " " + playerData.playerName;
        if (healthText != null) healthText.text = "HP: " + playerData.health + "/" + playerData.maxHealth;
        if (manaText != null) manaText.text = "MP: " + playerData.mana + "/" + playerData.maxMana;
        if (goldText != null) goldText.text = "Gold: " + playerData.gold;
        if (zoneText != null) zoneText.text = currentZone;
    }

    public void AddExperience(int amount)
    {
        playerData.exp += amount;
        while (playerData.exp >= playerData.expToNext)
        {
            playerData.exp -= playerData.expToNext;
            playerData.level++;
            playerData.expToNext = Mathf.FloorToInt(playerData.expToNext * 1.5f);
            playerData.maxHealth += 10; playerData.maxMana += 5;
            playerData.health = playerData.maxHealth; playerData.mana = playerData.maxMana;
        }
    }

    public void AddGold(int amt) => playerData.gold += amt;
    public bool SpendGold(int amt) { if (playerData.gold < amt) return false; playerData.gold -= amt; return true; }
    public void TakeDamage(int dmg) { playerData.health -= dmg; if (playerData.health <= 0) RespawnPlayer(); }
    public void HealPlayer(int amt) => playerData.health = Mathf.Min(playerData.maxHealth, playerData.health + amt);
    public void UseMana(int amt) => playerData.mana = Mathf.Max(0, playerData.mana - amt);
    public void RestoreMana(int amt) => playerData.mana = Mathf.Min(playerData.maxMana, playerData.mana + amt);
    public void CompleteQuest(string id) { if (!playerData.completedQuests.Contains(id)) playerData.completedQuests.Add(id); }
    public bool IsQuestCompleted(string id) => playerData.completedQuests.Contains(id);

    private void RespawnPlayer()
    {
        playerData.health = playerData.maxHealth / 2; playerData.mana = playerData.maxMana;
        PlayerController p = FindObjectOfType<PlayerController>();
        if (p != null) p.transform.position = Vector3.zero;
    }

    public void SaveGame() { PlayerPrefs.SetString("PlayerSave", JsonUtility.ToJson(playerData)); PlayerPrefs.Save(); }
    public void LoadGame() { if (PlayerPrefs.HasKey("PlayerSave")) playerData = JsonUtility.FromJson<PlayerSaveData>(PlayerPrefs.GetString("PlayerSave")); }
}
