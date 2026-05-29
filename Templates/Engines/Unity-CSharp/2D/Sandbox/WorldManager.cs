/// <summary>
/// Manages the sandbox world: block grid, placement, destruction, and world save/load state.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class WorldManager : MonoBehaviour
{
    [Header("World")]
    public int worldWidth = 100;
    public int worldHeight = 60;
    public float blockSize = 1f;
    private Dictionary<Vector2Int, Block> blockGrid = new Dictionary<Vector2Int, Block>();

    [Header("References")]
    public GameObject blockPrefab;
    public Transform blockParent;

    /// <summary>
    /// Initializes the world with a grass floor layer.
    /// </summary>
    private void Start()
    {
        // Create a grass floor across the bottom
        for (int x = 0; x < worldWidth; x++)
        {
            Vector2Int pos = new Vector2Int(x, 0);
            PlaceBlock(pos, Block.BlockType.Grass);
        }
        Debug.Log("World initialized: " + worldWidth + "x" + worldHeight);
    }

    /// <summary>
    /// Places a block of the given type at the specified grid position.
    /// </summary>
    public bool PlaceBlock(Vector2Int gridPos, Block.BlockType type)
    {
        if (blockGrid.ContainsKey(gridPos))
        {
            Debug.Log("Block already exists at " + gridPos);
            return false;
        }

        if (gridPos.x < 0 || gridPos.x >= worldWidth || gridPos.y < 0 || gridPos.y >= worldHeight)
            return false;

        // Instantiate the block at the grid position
        Vector3 worldPos = new Vector3(gridPos.x * blockSize, gridPos.y * blockSize, 0);
        GameObject blockObj = Instantiate(blockPrefab, worldPos, Quaternion.identity, blockParent);
        blockObj.transform.localScale = Vector3.one * blockSize;

        Block block = blockObj.GetComponent<Block>();
        block.Initialize(type, gridPos);

        blockGrid[gridPos] = block;
        return true;
    }

    /// <summary>
    /// Removes and destroys the block at the specified grid position.
    /// </summary>
    public bool DestroyBlock(Vector2Int gridPos)
    {
        if (!blockGrid.ContainsKey(gridPos))
            return false;

        Block block = blockGrid[gridPos];
        if (block.isIndestructible)
        {
            Debug.Log("Cannot destroy indestructible block at " + gridPos);
            return false;
        }

        blockGrid.Remove(gridPos);
        Destroy(block.gameObject);
        return true;
    }

    /// <summary>
    /// Returns the block at the given grid position, or null if empty.
    /// </summary>
    public Block GetBlockAt(Vector2Int gridPos)
    {
        blockGrid.TryGetValue(gridPos, out Block block);
        return block;
    }

    /// <summary>
    /// Converts a world-space position to grid coordinates.
    /// </summary>
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        int x = Mathf.FloorToInt(worldPos.x / blockSize);
        int y = Mathf.FloorToInt(worldPos.y / blockSize);
        return new Vector2Int(x, y);
    }
}
