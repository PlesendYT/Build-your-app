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

    public enum TileType { Wall, Floor, Door }

    public class Room
    {
        public int x, y, width, height;
        public Vector2Int Center => new Vector2Int(x + width / 2, y + height / 2);

        public bool Overlaps(Room other)
        {
            return x < other.x + other.width + 1 && x + width + 1 > other.x &&
                   y < other.y + other.height + 1 && y + height + 1 > other.y;
        }
    }

    private List<Room> rooms = new List<Room>();

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

    private void InitializeGrid()
    {
        for (int x = 0; x < gridWidth; x++)
            for (int y = 0; y < gridHeight; y++)
                grid[x, y] = TileType.Wall;
    }

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

    private void CarveRoom(Room room)
    {
        for (int x = room.x; x < room.x + room.width; x++)
            for (int y = room.y; y < room.y + room.height; y++)
                grid[x, y] = TileType.Floor;
    }

    private void ConnectRooms()
    {
        for (int i = 1; i < rooms.Count; i++)
        {
            Vector2Int start = rooms[i - 1].Center;
            Vector2Int end = rooms[i].Center;
            CarveCorridor(start.x, start.y, end.x, end.y);
        }
    }

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

    private void SpawnPlayer()
    {
        if (playerPrefab != null && rooms.Count > 0)
        {
            Vector2Int pos = rooms[0].Center;
            Instantiate(playerPrefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        }
    }

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

    public TileType GetTile(int x, int y)
    {
        if (x < 0 || x >= gridWidth || y < 0 || y >= gridHeight)
            return TileType.Wall;
        return grid[x, y];
    }
}
