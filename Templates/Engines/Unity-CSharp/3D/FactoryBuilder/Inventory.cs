/// <summary>
/// Factory storage management: input/output buffers, item filtering, and stack organization.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class StoredItem
{
    public string itemID, itemName;
    public int quantity, maxStack = 999;
    public StoredItem(string id, string name, int qty) { itemID = id; itemName = name; quantity = qty; }
}

public class Inventory : MonoBehaviour
{
    [Header("Storage")]
    public string storageName = "Storage Container";
    public int slotCount = 20;
    public string[] acceptedItems, bannedItems;

    [Header("IO")]
    public bool autoInput, autoOutput;
    public int outputRate = 60;
    private List<StoredItem> slots = new List<StoredItem>();
    private float outputTimer;

    private void Update()
    {
        if (autoOutput) AutoOutput();
    }

    public bool AddItem(string itemID, string itemName, int quantity)
    {
        if (!IsItemAccepted(itemID)) return false;
        foreach (StoredItem slot in slots)
            if (slot.itemID == itemID && slot.quantity < slot.maxStack)
            {
                int space = slot.maxStack - slot.quantity;
                int add = Mathf.Min(space, quantity);
                slot.quantity += add; quantity -= add;
                if (quantity <= 0) return true;
            }
        if (slots.Count < slotCount) { slots.Add(new StoredItem(itemID, itemName, quantity)); return true; }
        return false;
    }

    public bool RemoveItem(string itemID, int quantity)
    {
        foreach (StoredItem slot in slots)
            if (slot.itemID == itemID && slot.quantity >= quantity)
            {
                slot.quantity -= quantity;
                if (slot.quantity <= 0) slots.Remove(slot);
                return true;
            }
        return false;
    }

    public int GetItemCount(string itemID)
    {
        int count = 0;
        foreach (StoredItem s in slots) if (s.itemID == itemID) count += s.quantity;
        return count;
    }

    private bool IsItemAccepted(string itemID)
    {
        foreach (string b in bannedItems) if (b == itemID) return false;
        if (acceptedItems.Length > 0) { foreach (string a in acceptedItems) if (a == itemID) return true; return false; }
        return true;
    }

    private void AutoOutput()
    {
        outputTimer += Time.deltaTime;
        if (outputTimer >= 60f / outputRate && slots.Count > 0)
        {
            StoredItem item = slots[0]; item.quantity--;
            if (item.quantity <= 0) slots.RemoveAt(0);
            outputTimer = 0f;
        }
    }

    public List<StoredItem> GetSlots() => slots;
    public bool IsFull() => slots.Count >= slotCount;
}
