/// <summary>
/// MMO-style inventory with bag slots, equipment slots, item stacking, and bank storage.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Item
{
    public string itemID, itemName, description;
    public int quantity = 1, maxStack = 99, sellPrice, buyPrice, levelRequirement;
    public ItemType type = ItemType.Misc;
    public int strengthBonus, intBonus, defenseBonus;
    public Sprite icon;

    public enum ItemType { Weapon, Armor, Accessory, Consumable, QuestItem, Misc }
    public bool IsEquipment() => type == ItemType.Weapon || type == ItemType.Armor || type == ItemType.Accessory;
}

public class Inventory : MonoBehaviour
{
    [Header("Bag")]
    public int bagSlotCount = 40, gold;
    public List<Item> items = new List<Item>();

    [Header("Equipment")]
    public Item weaponSlot, helmetSlot, chestSlot, legsSlot, bootsSlot, accessorySlot;

    [Header("Bank")]
    public List<Item> bankStorage = new List<Item>();
    public int bankSlotCount = 80;

    public bool AddItem(Item item)
    {
        // Stack with existing
        if (item.maxStack > 1)
            foreach (Item ex in items)
                if (ex.itemID == item.itemID && ex.quantity < ex.maxStack)
                {
                    int space = ex.maxStack - ex.quantity;
                    int toAdd = Mathf.Min(space, item.quantity);
                    ex.quantity += toAdd; item.quantity -= toAdd;
                    if (item.quantity <= 0) return true;
                }

        if (items.Count < bagSlotCount) { items.Add(item); return true; }
        return false;
    }

    public bool RemoveItem(Item item, int count = 1)
    {
        if (!items.Contains(item) || item.quantity < count) return false;
        item.quantity -= count;
        if (item.quantity <= 0) items.Remove(item);
        return true;
    }

    public bool EquipItem(Item item)
    {
        if (!item.IsEquipment() || !items.Contains(item)) return false;
        switch (item.type)
        {
            case Item.ItemType.Weapon: if (weaponSlot != null) UnequipItem(weaponSlot); weaponSlot = item; break;
            case Item.ItemType.Armor:
                if (item.itemName.Contains("Helmet")) { if (helmetSlot != null) UnequipItem(helmetSlot); helmetSlot = item; }
                else if (item.itemName.Contains("Chest")) { if (chestSlot != null) UnequipItem(chestSlot); chestSlot = item; }
                else if (item.itemName.Contains("Legs")) { if (legsSlot != null) UnequipItem(legsSlot); legsSlot = item; }
                else if (item.itemName.Contains("Boots")) { if (bootsSlot != null) UnequipItem(bootsSlot); bootsSlot = item; }
                break;
            case Item.ItemType.Accessory: if (accessorySlot != null) UnequipItem(accessorySlot); accessorySlot = item; break;
        }
        items.Remove(item);
        return true;
    }

    public void UnequipItem(Item item)
    {
        if (item == null) return;
        AddItem(item);
        if (weaponSlot == item) weaponSlot = null;
        else if (helmetSlot == item) helmetSlot = null;
        else if (chestSlot == item) chestSlot = null;
        else if (legsSlot == item) legsSlot = null;
        else if (bootsSlot == item) bootsSlot = null;
        else if (accessorySlot == item) accessorySlot = null;
    }

    public bool CanAfford(int price) => gold >= price;
    public void AddGold(int amt) => gold += amt;
    public bool SpendGold(int amt) { if (gold < amt) return false; gold -= amt; return true; }
    public void DepositToBank(Item item) { if (items.Contains(item) && bankStorage.Count < bankSlotCount) { items.Remove(item); bankStorage.Add(item); } }
    public void WithdrawFromBank(Item item) { if (bankStorage.Contains(item) && items.Count < bagSlotCount) { bankStorage.Remove(item); items.Add(item); } }
}
