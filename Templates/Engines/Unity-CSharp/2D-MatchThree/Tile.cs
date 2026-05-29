/// <summary>
/// Represents a single tile in the match-three grid with position, type, selection highlight, and click handling.
/// </summary>
using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector] public int x;
    [HideInInspector] public int y;
    [HideInInspector] public int type;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    /// <summary>
    /// Caches sprite renderer and original color for selection highlight.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    /// <summary>
    /// Initializes the tile with grid coordinates and type.
    /// </summary>
    public void Init(int gridX, int gridY, int tileType)
    {
        x = gridX;
        y = gridY;
        type = tileType;
    }

    /// <summary>
    /// Moves the tile to a new grid position and updates its world position.
    /// </summary>
    public void MoveTo(int newX, int newY)
    {
        x = newX;
        y = newY;
        transform.position = new Vector3(newX, newY, 0f) + (transform.parent != null ? transform.parent.position : Vector3.zero);
    }

    /// <summary>
    /// Highlights the tile in yellow when selected, reverts to original color when deselected.
    /// </summary>
    public void SetSelected(bool selected)
    {
        spriteRenderer.color = selected ? Color.yellow : originalColor;
    }

    /// <summary>
    /// Notifies the BoardManager when the tile is clicked.
    /// </summary>
    private void OnMouseDown()
    {
        BoardManager board = FindObjectOfType<BoardManager>();
        if (board != null)
            board.SelectTile(this);
    }
}
