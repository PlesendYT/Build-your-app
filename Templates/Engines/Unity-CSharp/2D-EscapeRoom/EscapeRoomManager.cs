/// <summary>
/// Manages escape room puzzles: inventory, item combinations, puzzle solving, and win condition.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PuzzleRequirement
{
    public string requiredItem;
    public string targetItem;
    public string resultItem;
    public string successMessage;
}

public class EscapeRoomManager : MonoBehaviour
{
    [Header("Puzzle Data")]
    public List<PuzzleRequirement> combinations = new List<PuzzleRequirement>();
    public string[] solvedPuzzles;
    private HashSet<string> solvedSet = new HashSet<string>();
    private List<string> inventory = new List<string>();

    public TMPro.TextMeshProUGUI messageText;
    public GameObject winPanel;

    /// <summary>
    /// Singleton instance for global access.
    /// </summary>
    public static EscapeRoomManager Instance { get; private set; }

    /// <summary>
    /// Initializes the singleton instance.
    /// </summary>
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// Hides the win panel on start.
    /// </summary>
    private void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
    }

    /// <summary>
    /// Adds an item to the inventory and shows a pickup message.
    /// </summary>
    public void PickupItem(string itemName)
    {
        if (!inventory.Contains(itemName))
        {
            inventory.Add(itemName);
            ShowMessage("Picked up: " + itemName);
        }
    }

    /// <summary>
    /// Checks if an item is in the inventory.
    /// </summary>
    public bool HasItem(string itemName)
    {
        return inventory.Contains(itemName);
    }

    /// <summary>
    /// Removes an item from the inventory.
    /// </summary>
    public void UseItem(string itemName)
    {
        if (inventory.Contains(itemName))
            inventory.Remove(itemName);
    }

    /// <summary>
    /// Attempts to combine two items. If a valid combination is found, consumes the item and checks win condition.
    /// </summary>
    public bool TryCombine(string usedItem, string targetItem)
    {
        foreach (PuzzleRequirement req in combinations)
        {
            if (req.requiredItem == usedItem && req.targetItem == targetItem)
            {
                if (inventory.Contains(usedItem))
                {
                    inventory.Remove(usedItem);
                    if (!string.IsNullOrEmpty(req.resultItem))
                        inventory.Add(req.resultItem);
                    solvedSet.Add(usedItem + "+" + targetItem);
                    ShowMessage(req.successMessage);
                    CheckWinCondition();
                    return true;
                }
            }
        }
        ShowMessage("Nothing happens...");
        return false;
    }

    /// <summary>
    /// Returns whether a specific puzzle combination has been solved.
    /// </summary>
    public bool IsSolved(string puzzleId)
    {
        return solvedSet.Contains(puzzleId);
    }

    /// <summary>
    /// Checks if all puzzle combinations have been solved and shows the win panel if so.
    /// </summary>
    private void CheckWinCondition()
    {
        foreach (PuzzleRequirement req in combinations)
        {
            string key = req.requiredItem + "+" + req.targetItem;
            if (!solvedSet.Contains(key))
                return;
        }
        ShowMessage("You escaped!");
        if (winPanel != null)
            winPanel.SetActive(true);
    }

    /// <summary>
    /// Displays a temporary message and clears it after 3 seconds.
    /// </summary>
    public void ShowMessage(string msg)
    {
        if (messageText != null)
        {
            messageText.text = msg;
            Invoke(nameof(ClearMessage), 3f);
        }
    }

    /// <summary>
    /// Clears the message text.
    /// </summary>
    private void ClearMessage()
    {
        if (messageText != null)
            messageText.text = "";
    }
}
