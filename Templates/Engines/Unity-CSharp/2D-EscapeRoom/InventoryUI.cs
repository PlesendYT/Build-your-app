using UnityEngine;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public TMP_Text inventoryText;
    public GameObject inventoryPanel;

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
