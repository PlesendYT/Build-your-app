/// <summary>
/// Manages survival-crafting game state: player health, hunger, inventory, and crafting recipes.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [Header("Player Stats")]
    public float health = 100f;
    public float maxHealth = 100f;
    public float hunger = 100f;
    public float maxHunger = 100f;
    public float hungerDrain = 1f;

    [Header("Inventory")]
    public List<ItemStack> inventory = new List<ItemStack>();
    public int maxInventorySlots = 20;

    [Header("References")]
    public CraftingSystem craftingSystem;

    [System.Serializable]
    public class ItemStack
    {
        public string itemName;
        public int quantity;
    }

    /// <summary>
    /// Initializes crafting system reference.
    /// </summary>
    private void Start()
    {
        if (craftingSystem == null)
            craftingSystem = FindObjectOfType<CraftingSystem>();
    }

    /// <summary>
    /// Drains hunger over time and applies damage if starving.
    /// </summary>
    private void Update()
    {
        hunger = Mathf.Max(0, hunger - hungerDrain * Time.deltaTime);
        if (hunger <= 0)
        {
            health -= Time.deltaTime * 2f;
        }
    }

    /// <summary>
    /// Adds an item to the inventory; returns false if inventory is full.
    /// </summary>
    public bool AddItem(string name, int quantity)
    {
        foreach (ItemStack slot in inventory)
        {
            if (slot.itemName == name)
            {
                slot.quantity += quantity;
                return true;
            }
        }
        // Create a new slot if there's room
        if (inventory.Count < maxInventorySlots)
        {
            inventory.Add(new ItemStack { itemName = name, quantity = quantity });
            return true;
        }
        Debug.Log("Inventory full!");
        return false;
    }

    /// <summary>
    /// Removes a quantity of an item from the inventory; returns true if successful.
    /// </summary>
    public bool RemoveItem(string name, int quantity)
    {
        for (int i = 0; i < inventory.Count; i++)
        {
            if (inventory[i].itemName == name)
            {
                inventory[i].quantity -= quantity;
                if (inventory[i].quantity <= 0)
                    inventory.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Applies damage to the player, triggering death if health reaches zero.
    /// </summary>
    public void TakeDamage(float amount)
    {
        health = Mathf.Max(0, health - amount);
        if (health <= 0) Debug.Log("Player has died.");
    }
}
