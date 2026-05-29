/// <summary>
/// Manages a strategy game grid with procedural terrain generation using Perlin noise.
/// </summary>
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [Header("Grid")]
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;
    public Vector3 origin;

    public enum TileType { Plain, Forest, Mountain, Water }
    private TileType[,] tiles;

    /// <summary>
    /// Initializes the grid and generates procedural terrain.
    /// </summary>
    private void Awake()
    {
        tiles = new TileType[width, height];
        GenerateTerrain();
    }

    /// <summary>
    /// Uses Perlin noise to procedurally generate terrain types (water, forest, plain, mountain).
    /// </summary>
    private void GenerateTerrain()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                float noise = Mathf.PerlinNoise(x * 0.1f, y * 0.1f);
                tiles[x, y] = noise switch
                {
                    < 0.3f => TileType.Water,
                    < 0.5f => TileType.Forest,
                    < 0.8f => TileType.Plain,
                    _ => TileType.Mountain
                };
            }
        }
    }

    /// <summary>
    /// Converts grid coordinates to world position.
    /// </summary>
    public Vector3 GridToWorld(int x, int y)
    {
        return origin + new Vector3(x * cellSize, y * cellSize);
    }

    /// <summary>
    /// Converts a world position to the nearest grid coordinate.
    /// </summary>
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3 offset = worldPos - origin;
        return new Vector2Int(
            Mathf.RoundToInt(offset.x / cellSize),
            Mathf.RoundToInt(offset.y / cellSize)
        );
    }

    /// <summary>
    /// Checks whether the given coordinates are within the grid bounds.
    /// </summary>
    public bool IsValid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    /// <summary>
    /// Returns true if the tile is walkable (not water or mountain).
    /// </summary>
    public bool IsWalkable(int x, int y)
    {
        return IsValid(x, y) && tiles[x, y] != TileType.Water && tiles[x, y] != TileType.Mountain;
    }

    /// <summary>
    /// Returns the movement cost for a tile (1 for plain, 2 for forest, max for others).
    /// </summary>
    public int GetMovementCost(int x, int y)
    {
        if (!IsValid(x, y)) return int.MaxValue;
        return tiles[x, y] switch
        {
            TileType.Plain => 1,
            TileType.Forest => 2,
            _ => int.MaxValue
        };
    }

    /// <summary>
    /// Gets the tile type at the given position, returning Water if out of bounds.
    /// </summary>
    public TileType GetTileType(int x, int y)
    {
        if (!IsValid(x, y)) return TileType.Water;
        return tiles[x, y];
    }
}
