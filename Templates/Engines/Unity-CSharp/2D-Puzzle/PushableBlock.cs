/// <summary>
/// A pushable block that slides on the grid, tracks target tiles, and notifies the PuzzleManager.
/// </summary>
using UnityEngine;

public class PushableBlock : MonoBehaviour
{
    [Header("Settings")]
    public float moveDuration = 0.2f;
    public LayerMask obstacleLayer;

    private GridManager gridManager;
    private PuzzleManager puzzleManager;
    private Vector2Int gridPos;
    private bool isMoving;
    private bool onTarget;
    private Rigidbody2D rb;

    /// <summary>
    /// Caches component references and finds the grid and puzzle managers.
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gridManager = FindAnyObjectByType<GridManager>();
        puzzleManager = FindAnyObjectByType<PuzzleManager>();
    }

    /// <summary>
    /// Calculates the grid position and snaps to the grid.
    /// </summary>
    private void Start()
    {
        gridPos = gridManager.WorldToGrid(transform.position);
        AlignToGrid();
    }

    /// <summary>
    /// Snaps the block position to the grid.
    /// </summary>
    private void AlignToGrid()
    {
        transform.position = gridManager.GridToWorld(gridPos.x, gridPos.y);
    }

    /// <summary>
    /// Attempts to push the block in the given direction if valid and not already moving.
    /// </summary>
    public bool TryPush(Vector2Int direction)
    {
        if (isMoving) return false;

        Vector2Int newPos = gridPos + direction;

        if (!gridManager.IsWalkable(newPos.x, newPos.y))
            return false;

        Collider2D hit = Physics2D.OverlapPoint(
            gridManager.GridToWorld(newPos.x, newPos.y), obstacleLayer);
        if (hit != null) return false;

        StartCoroutine(MoveTo(newPos));
        return true;
    }

    /// <summary>
    /// Smoothly moves the block to the new grid position and checks if it lands on a target tile.
    /// </summary>
    private System.Collections.IEnumerator MoveTo(Vector2Int newPos)
    {
        isMoving = true;
        Vector2 startPos = transform.position;
        Vector2 endPos = gridManager.GridToWorld(newPos.x, newPos.y);
        float elapsed = 0f;

        if (onTarget)
        {
            onTarget = false;
            puzzleManager.RegisterBlockOffTarget();
        }

        while (elapsed < moveDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / moveDuration;
            transform.position = Vector2.Lerp(startPos, endPos, t);
            yield return null;
        }

        transform.position = endPos;
        gridPos = newPos;
        isMoving = false;
        puzzleManager.RegisterMove();

        GridManager.TileType tileType = gridManager.GetTileType(newPos.x, newPos.y);
        if (tileType == GridManager.TileType.Target)
        {
            onTarget = true;
            puzzleManager.RegisterBlockOnTarget();
        }
    }
}
