/// <summary>
/// Manages extraction shooter game state: player health, loot inventory, extraction logic, and raid timer.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Player")]
    public float health = 100f;
    public float maxHealth = 100f;

    [Header("Loot")]
    public List<LootItem> lootInventory = new List<LootItem>();
    public int maxLootSlots = 10;

    [Header("Raid")]
    public float raidTimeLimit = 600f;
    private float raidTimer;
    public bool isExtracted;

    [System.Serializable]
    public class LootItem
    {
        public string itemName;
        public int value;
        public int quantity;
    }

    /// <summary>
    /// Initializes the raid timer at start.
    /// </summary>
    private void Start() { raidTimer = raidTimeLimit; }

    /// <summary>
    /// Counts down the raid timer; triggers fail extraction when time runs out.
    /// </summary>
    private void Update()
    {
        if (isExtracted) return;
        raidTimer -= Time.deltaTime;
        if (raidTimer <= 0) { Debug.Log("Raid time expired!"); lootInventory.Clear(); isExtracted = true; }
    }

    /// <summary>
    /// Applies damage to the player, with death handling at zero health.
    /// </summary>
    public void TakeDamage(float amount)
    {
        health = Mathf.Max(0, health - amount);
        if (health <= 0) { Debug.Log("Player downed! Loot lost."); lootInventory.Clear(); }
    }

    /// <summary>
    /// Heals the player by a given amount, capped at max health.
    /// </summary>
    public void Heal(float amount) { health = Mathf.Min(maxHealth, health + amount); }

    /// <summary>
    /// Adds loot to the inventory; returns false if the inventory is full.
    /// </summary>
    public bool AddLoot(string name, int value, int quantity = 1)
    {
        foreach (LootItem item in lootInventory)
            if (item.itemName == name) { item.quantity += quantity; return true; }

        if (lootInventory.Count < maxLootSlots)
        {
            lootInventory.Add(new LootItem { itemName = name, value = value, quantity = quantity });
            return true;
        }
        Debug.Log("Loot inventory full!");
        return false;
    }

    /// <summary>
    /// Removes all loot and returns the total value for scoring at extraction.
    /// </summary>
    public int Extract()
    {
        if (health <= 0) return 0;
        isExtracted = true;
        int totalValue = 0;
        foreach (LootItem item in lootInventory) totalValue += item.value * item.quantity;
        Debug.Log("Extracted with " + totalValue + " worth of loot!");
        return totalValue;
    }

    /// <summary>
    /// Returns the remaining raid time as a formatted string.
    /// </summary>
    public string GetRemainingTime()
    {
        int m = Mathf.FloorToInt(raidTimer / 60), s = Mathf.FloorToInt(raidTimer % 60);
        return m.ToString("00") + ":" + s.ToString("00");
    }
}
