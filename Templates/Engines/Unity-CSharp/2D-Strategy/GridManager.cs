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

    private void Awake()
    {
        tiles = new TileType[width, height];
        GenerateTerrain();
    }

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

    public Vector3 GridToWorld(int x, int y)
    {
        return origin + new Vector3(x * cellSize, y * cellSize);
    }

    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3 offset = worldPos - origin;
        return new Vector2Int(
            Mathf.RoundToInt(offset.x / cellSize),
            Mathf.RoundToInt(offset.y / cellSize)
        );
    }

    public bool IsValid(int x, int y)
    {
        return x >= 0 && x < width && y >= 0 && y < height;
    }

    public bool IsWalkable(int x, int y)
    {
        return IsValid(x, y) && tiles[x, y] != TileType.Water && tiles[x, y] != TileType.Mountain;
    }

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

    public TileType GetTileType(int x, int y)
    {
        if (!IsValid(x, y)) return TileType.Water;
        return tiles[x, y];
    }
}
