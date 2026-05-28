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

    public static EscapeRoomManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (winPanel != null) winPanel.SetActive(false);
    }

    public void PickupItem(string itemName)
    {
        if (!inventory.Contains(itemName))
        {
            inventory.Add(itemName);
            ShowMessage("Picked up: " + itemName);
        }
    }

    public bool HasItem(string itemName)
    {
        return inventory.Contains(itemName);
    }

    public void UseItem(string itemName)
    {
        if (inventory.Contains(itemName))
            inventory.Remove(itemName);
    }

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

    public bool IsSolved(string puzzleId)
    {
        return solvedSet.Contains(puzzleId);
    }

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

    public void ShowMessage(string msg)
    {
        if (messageText != null)
        {
            messageText.text = msg;
            Invoke(nameof(ClearMessage), 3f);
        }
    }

    private void ClearMessage()
    {
        if (messageText != null)
            messageText.text = "";
    }
}
