/// <summary>
/// Inventory system with add/remove, slot limits, and UI refresh events.
/// </summary>
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string itemName;
    public int quantity;
    public Sprite icon;
}

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>();
    public int maxSlots = 20;

    public TMPro.TextMeshProUGUI inventoryText;

    public event System.Action OnInventoryChanged;

    public bool AddItem(Item item)
    {
        Item existing = items.Find(i => i.itemName == item.itemName);
        if (existing != null)
        {
            existing.quantity += item.quantity;
        }
        else if (items.Count < maxSlots)
        {
            items.Add(item);
        }
        else
        {
            Debug.Log("Inventory full!");
            return false;
        }

        OnInventoryChanged?.Invoke();
        UpdateUI();
        return true;
    }

    public void RemoveItem(string itemName, int quantity)
    {
        Item item = items.Find(i => i.itemName == itemName);
        if (item != null)
        {
            item.quantity -= quantity;
            if (item.quantity <= 0) items.Remove(item);
            OnInventoryChanged?.Invoke();
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (inventoryText != null)
        {
            inventoryText.text = "Inventory:\n";
            foreach (var item in items)
                inventoryText.text += item.itemName + " x" + item.quantity + "\n";
        }
    }
}

public class Inventory : MonoBehaviour
{
    [Header("Settings")]
    public int maxSlots = 20;
    public List<Item> items = new List<Item>();

    [Header("Equipment")]
    public Item equippedWeapon;
    public Item equippedArmor;

    public bool AddItem(Item item)
    {
        if (items.Count >= maxSlots) return false;

        Item existing = items.Find(i => i.itemName == item.itemName && i.type == item.type);
        if (existing != null)
        {
            existing.quantity += item.quantity;
        }
        else
        {
            items.Add(item);
        }
        return true;
    }

    public bool RemoveItem(string itemName, int quantity = 1)
    {
        Item item = items.Find(i => i.itemName == itemName);
        if (item == null || item.quantity < quantity) return false;

        item.quantity -= quantity;
        if (item.quantity <= 0) items.Remove(item);
        return true;
    }

    public void EquipWeapon(Item weapon)
    {
        if (weapon.type != Item.ItemType.Weapon) return;
        if (equippedWeapon != null) items.Add(equippedWeapon);
        equippedWeapon = weapon;
        items.Remove(weapon);
    }

    public void EquipArmor(Item armor)
    {
        if (armor.type != Item.ItemType.Armor) return;
        if (equippedArmor != null) items.Add(equippedArmor);
        equippedArmor = armor;
        items.Remove(armor);
    }

    public int GetItemCount(string itemName)
    {
        Item item = items.Find(i => i.itemName == itemName);
        return item?.quantity ?? 0;
    }
}
