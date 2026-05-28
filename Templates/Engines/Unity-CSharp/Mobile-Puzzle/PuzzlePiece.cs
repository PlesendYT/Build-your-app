using UnityEngine;

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

    public void TrySnap()
    {
        if (Vector2.Distance(transform.position, GetWorldPosition(correctPosition)) <= snapDistance)
        {
            transform.position = GetWorldPosition(correctPosition);
            isLocked = true;
            if (spriteRenderer != null)
                spriteRenderer.color = Color.green;
            puzzleManager.RegisterPiecePlaced();
        }
    }

    private Vector3 GetWorldPosition(Vector2Int pos)
    {
        return new Vector3(pos.x, pos.y, 0);
    }

    private void OnMouseDown()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = selectedColor;
    }

    private void OnMouseUp()
    {
        if (spriteRenderer != null && !isLocked)
            spriteRenderer.color = originalColor;
    }
}
