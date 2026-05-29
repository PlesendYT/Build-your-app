/// <summary>
/// Manages the creative inventory in the sandbox game, providing infinite blocks and item selection.
/// </summary>
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    [Header("Creative Inventory")]
    public List<Block.BlockType> availableBlocks = new List<Block.BlockType>();
    public int selectedIndex;

    [Header("UI")]
    public Transform hotbarUI;
    public GameObject slotPrefab;

    /// <summary>
    /// Populates the creative inventory with all block types.
    /// </summary>
    private void Start()
    {
        // Add all block types to the creative inventory
        availableBlocks.Add(Block.BlockType.Dirt);
        availableBlocks.Add(Block.BlockType.Grass);
        availableBlocks.Add(Block.BlockType.Stone);
        availableBlocks.Add(Block.BlockType.Wood);
        availableBlocks.Add(Block.BlockType.Sand);
        availableBlocks.Add(Block.BlockType.Water);
    }

    /// <summary>
    /// Selects the next block type in the inventory, wrapping around.
    /// </summary>
    public void SelectNext()
    {
        if (availableBlocks.Count == 0) return;
        selectedIndex = (selectedIndex + 1) % availableBlocks.Count;
        Debug.Log("Selected: " + GetSelectedBlock());
    }

    /// <summary>
    /// Selects the previous block type in the inventory.
    /// </summary>
    public void SelectPrevious()
    {
        if (availableBlocks.Count == 0) return;
        selectedIndex = (selectedIndex - 1 + availableBlocks.Count) % availableBlocks.Count;
        Debug.Log("Selected: " + GetSelectedBlock());
    }

    /// <summary>
    /// Returns the currently selected block type.
    /// </summary>
    public Block.BlockType GetSelectedBlock()
    {
        if (selectedIndex < 0 || selectedIndex >= availableBlocks.Count)
            return Block.BlockType.Dirt;
        return availableBlocks[selectedIndex];
    }

    /// <summary>
    /// Handles hotbar slot key presses (1-6) to select a block directly.
    /// </summary>
    public void OnHotbarSelect(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        // Parse the input to get the slot number (implementation depends on Input Action binding)
        int slot = Mathf.RoundToInt(context.ReadValue<float>()) - 1;
        if (slot >= 0 && slot < availableBlocks.Count)
        {
            selectedIndex = slot;
        }
    }
}
