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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        gridManager = FindAnyObjectByType<GridManager>();
        puzzleManager = FindAnyObjectByType<PuzzleManager>();
    }

    private void Start()
    {
        gridPos = gridManager.WorldToGrid(transform.position);
        AlignToGrid();
    }

    private void AlignToGrid()
    {
        transform.position = gridManager.GridToWorld(gridPos.x, gridPos.y);
    }

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
