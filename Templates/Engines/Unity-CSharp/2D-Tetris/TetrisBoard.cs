/// <summary>
/// Manages the Tetris board grid: piece placement, line clearing, and row shifting.
/// </summary>
using UnityEngine;

public class TetrisBoard : MonoBehaviour
{
    [Header("Board")]
    public int width = 10;
    public int height = 20;
    public float cellSize = 1f;
    public GameObject cellPrefab;
    public Transform boardParent;

    private Transform[,] grid;

    /// <summary>
    /// Initializes the grid array.
    /// </summary>
    private void Awake()
    {
        grid = new Transform[width, height];
    }

    /// <summary>
    /// Checks if a grid position is valid (empty and within bounds).
    /// </summary>
    public bool IsValidPosition(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= width || pos.y < 0) return false;
        if (pos.y >= height) return true;
        return grid[pos.x, pos.y] == null;
    }

    /// <summary>
    /// Places a piece's blocks into the grid, clears full lines, and spawns the next piece.
    /// </summary>
    public void PlacePiece(Transform pieceTransform)
    {
        foreach (Transform child in pieceTransform)
        {
            Vector2Int pos = RoundToGrid(child.position);
            if (pos.y >= 0 && pos.y < height)
                grid[pos.x, pos.y] = child;
        }
        ClearLines();
        FindObjectOfType<TetrominoSpawner>()?.SpawnPiece();
    }

    /// <summary>
    /// Checks all rows for completeness and clears them, shifting rows down.
    /// </summary>
    private void ClearLines()
    {
        for (int y = 0; y < height; y++)
        {
            if (IsLineFull(y))
            {
                ClearLine(y);
                ShiftLinesDown(y);
                y--;
                GameManager.Instance.AddScore(100);
            }
        }
    }

    /// <summary>
    /// Returns true if every cell in the row is occupied.
    /// </summary>
    private bool IsLineFull(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] == null) return false;
        }
        return true;
    }

    /// <summary>
    /// Destroys all blocks in the specified row.
    /// </summary>
    private void ClearLine(int y)
    {
        for (int x = 0; x < width; x++)
        {
            if (grid[x, y] != null)
            {
                Destroy(grid[x, y].gameObject);
                grid[x, y] = null;
            }
        }
    }

    /// <summary>
    /// Shifts all rows above the cleared line down by one.
    /// </summary>
    private void ShiftLinesDown(int clearedY)
    {
        for (int y = clearedY; y < height - 1; y++)
        {
            for (int x = 0; x < width; x++)
            {
                grid[x, y] = grid[x, y + 1];
                if (grid[x, y] != null)
                    grid[x, y].position += Vector3.down;
            }
        }
    }

    /// <summary>
    /// Converts a world position to the nearest grid coordinate relative to the board parent.
    /// </summary>
    public Vector2Int RoundToGrid(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x - boardParent.position.x);
        int y = Mathf.RoundToInt(position.y - boardParent.position.y);
        return new Vector2Int(x, y);
    }
}
