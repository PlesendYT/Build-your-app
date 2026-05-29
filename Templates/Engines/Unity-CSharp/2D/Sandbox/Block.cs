/// <summary>
/// Represents a single block in the sandbox world with type, color, and physical properties.
/// </summary>
using UnityEngine;

public class Block : MonoBehaviour
{
    [Header("Block Data")]
    public BlockType type = BlockType.Dirt;
    public Vector2Int gridPosition;
    public bool isIndestructible;
    public float hardness = 1f;

    [Header("Visual")]
    public SpriteRenderer blockRenderer;

    public enum BlockType
    {
        Dirt,
        Grass,
        Stone,
        Wood,
        Sand,
        Water
    }

    /// <summary>
    /// Caches the sprite renderer.
    /// </summary>
    private void Awake()
    {
        if (blockRenderer == null)
            blockRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Initializes this block with the given type and grid position.
    /// </summary>
    public void Initialize(BlockType blockType, Vector2Int pos)
    {
        type = blockType;
        gridPosition = pos;
        ApplyTypeProperties();
    }

    /// <summary>
    /// Applies visual and physical properties based on the block type.
    /// </summary>
    private void ApplyTypeProperties()
    {
        switch (type)
        {
            case BlockType.Dirt:
                blockRenderer.color = new Color(0.5f, 0.3f, 0.1f);
                hardness = 0.5f;
                isIndestructible = false;
                break;
            case BlockType.Grass:
                blockRenderer.color = new Color(0.2f, 0.7f, 0.1f);
                hardness = 0.6f;
                isIndestructible = true;
                break;
            case BlockType.Stone:
                blockRenderer.color = new Color(0.4f, 0.4f, 0.4f);
                hardness = 2f;
                isIndestructible = false;
                break;
            case BlockType.Wood:
                blockRenderer.color = new Color(0.6f, 0.4f, 0.2f);
                hardness = 1f;
                isIndestructible = false;
                break;
            case BlockType.Sand:
                blockRenderer.color = new Color(0.8f, 0.7f, 0.4f);
                hardness = 0.3f;
                isIndestructible = false;
                break;
            case BlockType.Water:
                blockRenderer.color = new Color(0.2f, 0.4f, 0.8f, 0.6f);
                hardness = 0f;
                isIndestructible = false;
                break;
        }
    }

    /// <summary>
    /// Called when a player or object interacts with this block.
    /// </summary>
    public void OnInteract()
    {
        Debug.Log("Interacted with " + type + " block at " + gridPosition);
    }

    /// <summary>
    /// Returns the block's name as a readable string.
    /// </summary>
    public string GetBlockName() { return type.ToString(); }
}
