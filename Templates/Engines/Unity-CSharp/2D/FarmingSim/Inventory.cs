/// <summary>
/// Manages the player's inventory of seeds, harvested crops, and tools in the farming sim.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [Header("Inventory")]
    public List<ItemSlot> slots = new List<ItemSlot>();
    public int maxSlots = 24;
    public int selectedSlot;

    [System.Serializable]
    public class ItemSlot
    {
        public string itemName;
        public int quantity;
        public Sprite icon;
    }

    /// <summary>
    /// Adds an item to the inventory; stacks into existing slots or creates a new one.
    /// </summary>
    public bool AddItem(string name, int quantity, Sprite icon = null)
    {
        // Try to stack first
        foreach (ItemSlot slot in slots)
        {
            if (slot.itemName == name)
            {
                slot.quantity += quantity;
                return true;
            }
        }

        // Create a new slot if there's room
        if (slots.Count < maxSlots)
        {
            slots.Add(new ItemSlot { itemName = name, quantity = quantity, icon = icon });
            return true;
        }

        Debug.Log("Inventory full!");
        return false;
    }

    /// <summary>
    /// Removes one instance of the specified item; returns false if not found.
    /// </summary>
    public bool RemoveItem(string name, int quantity = 1)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].itemName == name)
            {
                slots[i].quantity -= quantity;
                if (slots[i].quantity <= 0)
                    slots.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Returns whether the player has at least the given quantity of an item.
    /// </summary>
    public bool HasItem(string name, int quantity = 1)
    {
        ItemSlot slot = slots.Find(s => s.itemName == name);
        return slot != null && slot.quantity >= quantity;
    }

    /// <summary>
    /// Returns the currently selected item in the hotbar.
    /// </summary>
    public ItemSlot GetSelectedItem()
    {
        if (selectedSlot >= 0 && selectedSlot < slots.Count)
            return slots[selectedSlot];
        return null;
    }
}
