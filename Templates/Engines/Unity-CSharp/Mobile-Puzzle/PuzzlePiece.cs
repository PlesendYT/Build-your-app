using UnityEngine;

/// <summary>
/// Represents a single puzzle piece with grid position, snap behavior, and visual feedback.
/// </summary>
public class PuzzlePiece : MonoBehaviour
{
    [Header("Settings")]
    public int pieceId;
    public Vector2Int gridPosition;
    public Vector2Int correctPosition;
    public bool isLocked;
    public float snapDistance = 0.5f;

    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Color selectedColor = Color.yellow;

    private Color originalColor;
    private PuzzleManager puzzleManager;

    private void Awake()
    {
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
        puzzleManager = FindAnyObjectByType<PuzzleManager>();
    }

    /// <summary>
    /// Attempts to snap the piece to its correct position if close enough.
    /// </summary>
    public void TrySnap()
    {
        if (Vector2.Distance(transform.position, GetWorldPosition(correctPosition)) <= snapDistance)
        {
            // Snap to correct position and lock the piece
            transform.position = GetWorldPosition(correctPosition);
            isLocked = true;
            if (spriteRenderer != null)
                spriteRenderer.color = Color.green;
            puzzleManager.RegisterPiecePlaced();
        }
    }

    /// <summary>
    /// Converts a grid position to a world-space Vector3.
    /// </summary>
    private Vector3 GetWorldPosition(Vector2Int pos)
    {
        return new Vector3(pos.x, pos.y, 0);
    }

    private void OnMouseDown()
    {
        // Highlight piece when selected
        if (spriteRenderer != null)
            spriteRenderer.color = selectedColor;
    }

    private void OnMouseUp()
    {
        // Restore original color when deselected, unless piece is locked
        if (spriteRenderer != null && !isLocked)
            spriteRenderer.color = originalColor;
    }
}
