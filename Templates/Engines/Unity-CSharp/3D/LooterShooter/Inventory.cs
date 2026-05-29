/// <summary>
/// Grid-based inventory system with item stacking, sorting, and equipment slots for looter shooter.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class InventoryItem
{
    public string itemName;
    public string itemID;
    public int quantity;
    public int maxStack = 99;
    public int gridWidth = 1;
    public int gridHeight = 1;
    public Sprite icon;
    public bool isEquippable;
}

public class Inventory : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridWidth = 10;
    public int gridHeight = 6;

    [Header("Equipment Slots")]
    public InventoryItem weaponSlot;
    public InventoryItem armorSlot;
    public InventoryItem accessorySlot;

    // Grid: true means occupied
    private bool[,] grid;
    private List<InventoryItem> items = new List<InventoryItem>();

    private void Awake()
    {
        grid = new bool[gridWidth, gridHeight];
    }

    public bool AddItem(InventoryItem item)
    {
        // Try stacking first
        foreach (InventoryItem existing in items)
        {
            if (existing.itemID == item.itemID && existing.quantity < existing.maxStack)
            {
                int space = existing.maxStack - existing.quantity;
                int toAdd = Mathf.Min(space, item.quantity);
                existing.quantity += toAdd;
                item.quantity -= toAdd;
                if (item.quantity <= 0) return true;
            }
        }

        // Find free grid position
        for (int y = 0; y <= gridHeight - item.gridHeight; y++)
        {
            for (int x = 0; x <= gridWidth - item.gridWidth; x++)
            {
                if (IsSpaceFree(x, y, item.gridWidth, item.gridHeight))
                {
                    OccupySpace(x, y, item.gridWidth, item.gridHeight);
                    items.Add(item);
                    return true;
                }
            }
        }
        return false; // No space
    }

    public bool RemoveItem(InventoryItem item, int count = 1)
    {
        if (item.quantity < count) return false;
        item.quantity -= count;
        if (item.quantity <= 0)
        {
            items.Remove(item);
        }
        return true;
    }

    public void EquipItem(InventoryItem item)
    {
        if (!item.isEquippable) return;
        // Equipment type determined by name prefix
        if (item.itemName.StartsWith("Weapon")) weaponSlot = item;
        else if (item.itemName.StartsWith("Armor")) armorSlot = item;
        else if (item.itemName.StartsWith("Accessory")) accessorySlot = item;
    }

    private bool IsSpaceFree(int startX, int startY, int width, int height)
    {
        for (int y = startY; y < startY + height; y++)
            for (int x = startX; x < startX + width; x++)
                if (grid[x, y]) return false;
        return true;
    }

    private void OccupySpace(int startX, int startY, int width, int height)
    {
        for (int y = startY; y < startY + height; y++)
            for (int x = startX; x < startX + width; x++)
                grid[x, y] = true;
    }

    public List<InventoryItem> GetItems() => items;
}
