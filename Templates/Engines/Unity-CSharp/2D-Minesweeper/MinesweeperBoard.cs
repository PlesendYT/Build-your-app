/// <summary>
/// Manages the minesweeper board: grid creation, mine placement, cell revealing with flood-fill, and win/lose detection.
/// </summary>
using UnityEngine;

public class MinesweeperBoard : MonoBehaviour
{
    [Header("Grid")]
    public int width = 9;
    public int height = 9;
    public int mineCount = 10;
    public GameObject cellPrefab;
    public Transform boardParent;
    public float cellSize = 1f;

    private Cell[,] grid;
    private bool isInitialized;
    private int revealedCount;
    private int totalSafeCells;

    /// <summary>
    /// Creates the cell grid and calculates the total number of safe cells.
    /// </summary>
    public void Init()
    {
        grid = new Cell[width, height];
        totalSafeCells = width * height - mineCount;

        float startX = -(width - 1) * cellSize / 2f;
        float startY = (height - 1) * cellSize / 2f;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(startX + x * cellSize, startY - y * cellSize, 0f);
                GameObject obj = Instantiate(cellPrefab, pos, Quaternion.identity, boardParent);
                Cell cell = obj.GetComponent<Cell>();
                cell.Init(x, y, this);
                grid[x, y] = cell;
            }
        }
    }

    /// <summary>
    /// Randomly places mines avoiding the first-click cell, then calculates adjacent mine counts.
    /// </summary>
    public void PlaceMines(int excludeX, int excludeY)
    {
        int placed = 0;
        while (placed < mineCount)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);
            if ((x == excludeX && y == excludeY) || grid[x, y].HasMine) continue;
            grid[x, y].HasMine = true;
            placed++;
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                grid[x, y].AdjacentMines = CountAdjacentMines(x, y);
            }
        }

        isInitialized = true;
    }

    /// <summary>
    /// Counts the number of mines adjacent to a given cell.
    /// </summary>
    private int CountAdjacentMines(int x, int y)
    {
        int count = 0;
        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                int nx = x + dx, ny = y + dy;
                if (nx >= 0 && nx < width && ny >= 0 && ny < height && grid[nx, ny].HasMine)
                    count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Reveals a cell: on first click places mines, on mine triggers game over, on empty cell flood-fills.
    /// </summary>
    public void RevealCell(int x, int y)
    {
        if (!isInitialized) PlaceMines(x, y);

        Cell cell = grid[x, y];
        if (cell.IsRevealed || cell.IsFlagged) return;

        cell.Reveal();

        if (cell.HasMine)
        {
            RevealAllMines();
            GameManager.Instance.GameOver(false);
            return;
        }

        revealedCount++;
        if (revealedCount >= totalSafeCells)
        {
            GameManager.Instance.GameOver(true);
            return;
        }

        // Flood-fill adjacent cells if this cell has no adjacent mines
        if (cell.AdjacentMines == 0)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    int nx = x + dx, ny = y + dy;
                    if (nx >= 0 && nx < width && ny >= 0 && ny < height)
                        RevealCell(nx, ny);
                }
            }
        }
    }

    /// <summary>
    /// Toggles a flag on a cell (right-click).
    /// </summary>
    public void ToggleFlag(int x, int y)
    {
        if (!isInitialized) return;
        Cell cell = grid[x, y];
        if (!cell.IsRevealed)
            cell.ToggleFlag();
    }

    /// <summary>
    /// Reveals all mines on the board (called on game loss).
    /// </summary>
    private void RevealAllMines()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (grid[x, y].HasMine)
                    grid[x, y].Reveal();
            }
        }
    }
}
