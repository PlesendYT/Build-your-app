/// <summary>
/// Manages a hidden object scene: randomly selects items to find, tracks progress, and provides hints.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class HiddenObjectScene : MonoBehaviour
{
    [Header("Scene Settings")]
    public HiddenItem[] allItems;
    public int itemsToFind = 5;
    public TMPro.TextMeshProUGUI itemListText;
    public GameObject hintButton;

    private List<HiddenItem> activeItems = new List<HiddenItem>();
    private int foundCount;

    /// <summary>
    /// Randomly selects a subset of items to be active and shows the item list.
    /// </summary>
    private void Start()
    {
        List<HiddenItem> pool = new List<HiddenItem>(allItems);
        for (int i = 0; i < itemsToFind && pool.Count > 0; i++)
        {
            int idx = Random.Range(0, pool.Count);
            activeItems.Add(pool[idx]);
            pool[idx].gameObject.SetActive(true);
            pool.RemoveAt(idx);
        }
        UpdateUI();
    }

    /// <summary>
    /// Marks an item as found, removes it from the active list, and checks for level completion.
    /// </summary>
    public void ItemFound(HiddenItem item)
    {
        if (activeItems.Contains(item))
        {
            activeItems.Remove(item);
            foundCount++;
            item.OnFound();
            UpdateUI();

            if (activeItems.Count == 0)
                GameManager.Instance.LevelComplete();
        }
    }

    /// <summary>
    /// Flashes a random active item as a hint to the player.
    /// </summary>
    public void ShowHint()
    {
        if (activeItems.Count > 0)
        {
            HiddenItem target = activeItems[Random.Range(0, activeItems.Count)];
            target.FlashHint();
        }
    }

    /// <summary>
    /// Updates the item list UI with found count and remaining items.
    /// </summary>
    private void UpdateUI()
    {
        if (itemListText != null)
        {
            string text = "Find: " + foundCount + "/" + itemsToFind + "\n";
            foreach (HiddenItem item in activeItems)
                text += "- " + item.itemName + "\n";
            itemListText.text = text;
        }
    }
}
