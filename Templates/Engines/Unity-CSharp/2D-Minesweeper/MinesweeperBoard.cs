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

    public void ToggleFlag(int x, int y)
    {
        if (!isInitialized) return;
        Cell cell = grid[x, y];
        if (!cell.IsRevealed)
            cell.ToggleFlag();
    }

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
