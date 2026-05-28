using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Dungeon")]
    public int width = 20;
    public int height = 20;
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;
    public GameObject treasurePrefab;
    public Transform dungeonParent;

    public TileType[,] dungeon;
    private Vector2Int playerStart;

    public enum TileType { Wall, Floor }

    private void Start()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        dungeon = new TileType[width, height];

        for (int x = 0; x < width; x++)
            for (int y = 0; y < height; y++)
                dungeon[x, y] = TileType.Wall;

        int rooms = Random.Range(5, 10);
        List<Rect> roomRects = new List<Rect>();

        for (int i = 0; i < rooms; i++)
        {
            int roomW = Random.Range(4, 9);
            int roomH = Random.Range(4, 9);
            int roomX = Random.Range(1, width - roomW - 1);
            int roomY = Random.Range(1, height - roomH - 1);
            Rect room = new Rect(roomX, roomY, roomW, roomH);

            bool overlaps = false;
            foreach (Rect r in roomRects)
            {
                if (room.Overlaps(r))
                {
                    overlaps = true;
                    break;
                }
            }
            if (overlaps) continue;

            roomRects.Add(room);

            for (int x = roomX; x < roomX + roomW; x++)
                for (int y = roomY; y < roomY + roomH; y++)
                    dungeon[x, y] = TileType.Floor;
        }

        for (int i = 1; i < roomRects.Count; i++)
        {
            Vector2Int centerA = new Vector2Int(
                Mathf.RoundToInt(roomRects[i - 1].center.x),
                Mathf.RoundToInt(roomRects[i - 1].center.y)
            );
            Vector2Int centerB = new Vector2Int(
                Mathf.RoundToInt(roomRects[i].center.x),
                Mathf.RoundToInt(roomRects[i].center.y)
            );

            int cx = centerA.x;
            int cy = centerA.y;

            while (cx != centerB.x)
            {
                dungeon[cx, cy] = TileType.Floor;
                cx += cx < centerB.x ? 1 : -1;
            }
            while (cy != centerB.y)
            {
                dungeon[cx, cy] = TileType.Floor;
                cy += cy < centerB.y ? 1 : -1;
            }
        }

        RenderDungeon();

        playerStart = new Vector2Int(
            Mathf.RoundToInt(roomRects[0].center.x),
            Mathf.RoundToInt(roomRects[0].center.y)
        );

        Instantiate(playerPrefab, new Vector3(playerStart.x, playerStart.y, 0f), Quaternion.identity);

        for (int i = 1; i < roomRects.Count; i++)
        {
            if (Random.value < 0.5f)
            {
                Vector2Int enemyPos = new Vector2Int(
                    Mathf.RoundToInt(roomRects[i].center.x) + Random.Range(-1, 2),
                    Mathf.RoundToInt(roomRects[i].center.y) + Random.Range(-1, 2)
                );
                if (dungeon[enemyPos.x, enemyPos.y] == TileType.Floor)
                    Instantiate(enemyPrefab, new Vector3(enemyPos.x, enemyPos.y, 0f), Quaternion.identity);
            }
        }

        Vector2Int treasurePos = new Vector2Int(
            Mathf.RoundToInt(roomRects[roomRects.Count - 1].center.x),
            Mathf.RoundToInt(roomRects[roomRects.Count - 1].center.y)
        );
        Instantiate(treasurePrefab, new Vector3(treasurePos.x, treasurePos.y, 0f), Quaternion.identity);
    }

    private void RenderDungeon()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject prefab = dungeon[x, y] == TileType.Floor ? floorPrefab : wallPrefab;
                Instantiate(prefab, new Vector3(x, y, 0f), Quaternion.identity, dungeonParent);
            }
        }
    }

    public Vector2Int GetPlayerStart() => playerStart;
}
