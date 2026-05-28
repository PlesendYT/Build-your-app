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

    public void ShowHint()
    {
        if (activeItems.Count > 0)
        {
            HiddenItem target = activeItems[Random.Range(0, activeItems.Count)];
            target.FlashHint();
        }
    }

    private void UpdateUI()
    {
        if (itemListText != null)
        {
            string text = "Finde: " + foundCount + "/" + itemsToFind + "\n";
            foreach (HiddenItem item in activeItems)
                text += "- " + item.itemName + "\n";
            itemListText.text = text;
        }
    }
}
