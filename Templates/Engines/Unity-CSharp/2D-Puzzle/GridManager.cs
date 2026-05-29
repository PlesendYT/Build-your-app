/// <summary>
/// Manages a 2D tile grid for puzzle games with coordinate conversion, tile types, and level generation.
/// </summary>
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridWidth = 8;
    public int gridHeight = 8;
    public float cellSize = 1f;
    public Vector2 gridOrigin = Vector2.zero;

    [Header("Tile Prefabs")]
    public GameObject floorTilePrefab;
    public GameObject wallTilePrefab;
    public GameObject targetTilePrefab;

    private TileType[,] grid;

    /// <summary>
    /// Defines the types of tiles that can exist on the grid.
    /// </summary>
    public enum TileType { Empty, Floor, Wall, Target }

    /// <summary>
    /// Initializes the grid array.
    /// </summary>
    private void Awake()
    {
        grid = new TileType[gridWidth, gridHeight];
    }

    /// <summary>
    /// Converts grid coordinates to world position.
    /// </summary>
    public Vector2 GridToWorld(int x, int y)
    {
        return gridOrigin + new Vector2(x * cellSize, y * cellSize);
    }

    /// <summary>
    /// Converts a world position to the nearest grid coordinate.
    /// </summary>
    public Vector2Int WorldToGrid(Vector2 worldPos)
    {
        Vector2 offset = worldPos - gridOrigin;
        int x = Mathf.RoundToInt(offset.x / cellSize);
        int y = Mathf.RoundToInt(offset.y / cellSize);
        return new Vector2Int(x, y);
    }

    /// <summary>
    /// Checks whether the given coordinates are within the grid bounds.
    /// </summary>
    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
    }

    /// <summary>
    /// Returns true if the tile is not a wall and is within bounds.
    /// </summary>
    public bool IsWalkable(int x, int y)
    {
        return IsValidPosition(x, y) && grid[x, y] != TileType.Wall;
    }

    /// <summary>
    /// Gets the tile type at the given position, returning Wall if out of bounds.
    /// </summary>
    public TileType GetTileType(int x, int y)
    {
        if (!IsValidPosition(x, y)) return TileType.Wall;
        return grid[x, y];
    }

    /// <summary>
    /// Sets the tile type at the given position.
    /// </summary>
    public void SetTileType(int x, int y, TileType type)
    {
        if (IsValidPosition(x, y))
            grid[x, y] = type;
    }

    /// <summary>
    /// Parses a text level data file and spawns the corresponding tiles.
    /// </summary>
    public void GenerateLevel(TextAsset levelData)
    {
        string[] lines = levelData.text.Split('\n');
        for (int y = 0; y < Mathf.Min(lines.Length, gridHeight); y++)
        {
            string line = lines[y].Trim();
            for (int x = 0; x < Mathf.Min(line.Length, gridWidth); x++)
            {
                char c = line[x];
                TileType type = c switch
                {
                    '#' => TileType.Wall,
                    '.' => TileType.Floor,
                    'T' => TileType.Target,
                    _ => TileType.Empty
                };
                grid[x, y] = type;
                SpawnTile(x, y, type);
            }
        }
    }

    /// <summary>
    /// Instantiates the appropriate tile prefab at the given grid position.
    /// </summary>
    private void SpawnTile(int x, int y, TileType type)
    {
        Vector2 position = GridToWorld(x, y);
        GameObject prefab = type switch
        {
            TileType.Wall => wallTilePrefab,
            TileType.Target => targetTilePrefab,
            TileType.Floor => floorTilePrefab,
            _ => null
        };

        if (prefab != null)
        {
            Instantiate(prefab, position, Quaternion.identity, transform);
        }
    }
}
