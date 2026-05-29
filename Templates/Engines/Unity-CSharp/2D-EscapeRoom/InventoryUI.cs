/// <summary>
/// Displays the player's inventory items on the UI (currently shows a placeholder).
/// </summary>
using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TMP_Text inventoryText;
    public GameObject inventoryPanel;

    /// <summary>
    /// Updates the inventory text display each frame (placeholder implementation).
    /// </summary>
    private void Update()
    {
        if (inventoryText != null)
        {
            string text = "Inventory:\n";
            text += "- Nothing -\n";
            inventoryText.text = text;
        }
    }
}
