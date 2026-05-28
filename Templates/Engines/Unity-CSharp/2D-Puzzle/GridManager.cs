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

    public enum TileType { Empty, Floor, Wall, Target }

    private void Awake()
    {
        grid = new TileType[gridWidth, gridHeight];
    }

    public Vector2 GridToWorld(int x, int y)
    {
        return gridOrigin + new Vector2(x * cellSize, y * cellSize);
    }

    public Vector2Int WorldToGrid(Vector2 worldPos)
    {
        Vector2 offset = worldPos - gridOrigin;
        int x = Mathf.RoundToInt(offset.x / cellSize);
        int y = Mathf.RoundToInt(offset.y / cellSize);
        return new Vector2Int(x, y);
    }

    public bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < gridWidth && y >= 0 && y < gridHeight;
    }

    public bool IsWalkable(int x, int y)
    {
        return IsValidPosition(x, y) && grid[x, y] != TileType.Wall;
    }

    public TileType GetTileType(int x, int y)
    {
        if (!IsValidPosition(x, y)) return TileType.Wall;
        return grid[x, y];
    }

    public void SetTileType(int x, int y, TileType type)
    {
        if (IsValidPosition(x, y))
            grid[x, y] = type;
    }

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
