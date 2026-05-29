/// <summary>
/// Procedurally generates a dungeon with rooms, corridors, player start, and enemy placement.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Generation Settings")]
    public int gridWidth = 50;
    public int gridHeight = 50;
    public int roomMinSize = 5;
    public int roomMaxSize = 10;
    public int maxRooms = 10;
    public int corridorWidth = 1;

    [Header("Tile Prefabs")]
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject doorPrefab;
    public GameObject playerPrefab;
    public GameObject[] enemyPrefabs;

    private TileType[,] grid;

    /// <summary>
    /// Types of tiles in the dungeon.
    /// </summary>
    public enum TileType { Wall, Floor, Door }

    /// <summary>
    /// Represents a rectangular room with position, size, and overlap checking.
    /// </summary>
    public class Room
    {
        public int x, y, width, height;
        public Vector2Int Center => new Vector2Int(x + width / 2, y + height / 2);

        /// <summary>
        /// Checks if this room overlaps with another, including a 1-tile margin.
        /// </summary>
        public bool Overlaps(Room other)
        {
            return x < other.x + other.width + 1 && x + width + 1 > other.x &&
                   y < other.y + other.height + 1 && y + height + 1 > other.y;
        }
    }

    private List<Room> rooms = new List<Room>();

    /// <summary>
    /// Generates the dungeon: fills with walls, creates rooms, connects with corridors, and spawns entities.
    /// </summary>
    private void Start()
    {
        grid = new TileType[gridWidth, gridHeight];
        InitializeGrid();
        GenerateRooms();
        ConnectRooms();
        RenderGrid();
        SpawnPlayer();
        SpawnEnemies();
    }

    /// <summary>
    /// Fills the entire grid with wall tiles.
    /// </summary>
    private void InitializeGrid()
    {
        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
                grid[x, y] = TileType.Wall;
    }

    /// <summary>
    /// Randomly places non-overlapping rooms up to the maximum count.
    /// </summary>
    private void GenerateRooms()
    {
        for (int i = 0; i < maxRooms * 10; i++)
        {
            if (rooms.Count >= maxRooms) break;

            int w = Random.Range(roomMinSize, roomMaxSize);
            int h = Random.Range(roomMinSize, roomMaxSize);
            int x = Random.Range(1, gridWidth - w - 1);
            int y = Random.Range(1, gridHeight - h - 1);

            Room newRoom = new Room { x = x, y = y, width = w, height = h };

            bool overlaps = false;
            foreach (Room room in rooms)
            {
                if (room.Overlaps(newRoom))
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                rooms.Add(newRoom);
                CarveRoom(newRoom);
            }
        }
    }

    /// <summary>
    /// Converts the room area from walls to floor tiles.
    /// </summary>
    private void CarveRoom(Room room)
    {
        for (int x = room.x; x < room.x + room.width; x++)
            for (int y = room.y; y < room.y + room.height; y++)
                grid[x, y] = TileType.Floor;
    }

    /// <summary>
    /// Connects rooms sequentially with L-shaped corridors from center to center.
    /// </summary>
    private void ConnectRooms()
    {
        for (int i = 1; i < rooms.Count; i++)
        {
            Vector2Int start = rooms[i - 1].Center;
            Vector2Int end = rooms[i].Center;
            CarveCorridor(start.x, start.y, end.x, end.y);
        }
    }

    /// <summary>
    /// Carves an L-shaped corridor (horizontal then vertical) between two points.
    /// </summary>
    private void CarveCorridor(int x1, int y1, int x2, int y2)
    {
        int x = x1;
        int y = y1;

        while (x != x2)
        {
            grid[x, y] = TileType.Floor;
            x += x < x2 ? 1 : -1;
        }

        while (y != y2)
        {
            grid[x, y] = TileType.Floor;
            y += y < y2 ? 1 : -1;
        }
    }

    /// <summary>
    /// Instantiates tile prefabs for every cell in the grid.
    /// </summary>
    private void RenderGrid()
    {
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                GameObject prefab = grid[x, y] switch
                {
                    TileType.Floor => floorPrefab,
                    _ => wallPrefab
                };
                if (prefab != null)
                {
                    Instantiate(prefab, new Vector3(x, y, 0), Quaternion.identity, transform);
                }
            }
        }
    }

    /// <summary>
    /// Places the player in the center of the first room.
    /// </summary>
    private void SpawnPlayer()
    {
        if (playerPrefab != null && rooms.Count > 0)
        {
            Vector2Int pos = rooms[0].Center;
            Instantiate(playerPrefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        }
    }

    /// <summary>
    /// Spawns enemies randomly in rooms (excluding the first room).
    /// </summary>
    private void SpawnEnemies()
    {
        if (enemyPrefabs.Length == 0) return;

        for (int i = 1; i < rooms.Count; i++)
        {
            if (Random.value > 0.6f) continue;
            Vector2Int pos = rooms[i].Center;
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(prefab, new Vector3(pos.x + Random.Range(-1, 2), pos.y + Random.Range(-1, 2), 0), Quaternion.identity);
        }
    }

    /// <summary>
    /// Returns the tile type at the given grid position (Wall if out of bounds).
    /// </summary>
    public TileType GetTile(int x, int y)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
            return TileType.Wall;
        return grid[x, y];
    }
}
