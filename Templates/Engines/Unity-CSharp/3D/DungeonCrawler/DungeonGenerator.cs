/// <summary>
/// Procedural dungeon generator creating rooms, corridors, enemy spawns, and loot placement.
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Room Settings")]
    public int roomCount = 10;
    public int minRoomSize = 5;
    public int maxRoomSize = 10;
    public GameObject roomPrefab;
    public GameObject corridorPrefab;
    public GameObject exitPrefab;

    [Header("Enemies")]
    public GameObject[] enemyPrefabs;
    public int enemiesPerRoom = 3;

    [Header("Loot")]
    public GameObject[] lootPrefabs;
    public int lootPerRoom = 2;

    private List<Room> rooms = new List<Room>();
    private GameObject exitDoor;

    public class Room
    {
        public Bounds bounds;
        public Vector3 center;
        public List<GameObject> enemies = new List<GameObject>();
        public List<GameObject> loot = new List<GameObject>();
        public bool isCleared;
    }

    public void GenerateDungeon()
    {
        // Clear existing dungeon
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        rooms.Clear();

        // Generate rooms using random placement
        for (int i = 0; i < roomCount; i++)
        {
            Vector3 position = new Vector3(
                Random.Range(-20f, 20f),
                0f,
                Random.Range(-20f, 20f)
            );

            int width = Random.Range(minRoomSize, maxRoomSize + 1);
            int depth = Random.Range(minRoomSize, maxRoomSize + 1);

            Room room = new Room
            {
                center = position,
                bounds = new Bounds(position, new Vector3(width, 3f, depth))
            };

            // Place room floor
            GameObject roomObj = Instantiate(roomPrefab, position, Quaternion.identity, transform);
            roomObj.transform.localScale = new Vector3(width, 1f, depth);

            // Spawn enemies and loot
            if (i > 0) // Skip first room (player spawn)
            {
                for (int e = 0; e < enemiesPerRoom; e++)
                {
                    Vector3 enemyPos = position + new Vector3(Random.Range(-width / 2f + 1f, width / 2f - 1f), 0.5f, Random.Range(-depth / 2f + 1f, depth / 2f - 1f));
                    GameObject enemy = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], enemyPos, Quaternion.identity, transform);
                    room.enemies.Add(enemy);
                }

                for (int l = 0; l < lootPerRoom; l++)
                {
                    Vector3 lootPos = position + new Vector3(Random.Range(-width / 2f + 1f, width / 2f - 1f), 0.5f, Random.Range(-depth / 2f + 1f, depth / 2f - 1f));
                    GameObject loot = Instantiate(lootPrefabs[Random.Range(0, lootPrefabs.Length)], lootPos, Quaternion.identity, transform);
                    room.loot.Add(loot);
                }
            }

            rooms.Add(room);
        }

        // Place exit in last room
        Room lastRoom = rooms[rooms.Count - 1];
        exitDoor = Instantiate(exitPrefab, lastRoom.center + Vector3.forward * 3f, Quaternion.identity, transform);
        exitDoor.SetActive(false);
    }

    public void ActivateExit()
    {
        if (exitDoor != null) exitDoor.SetActive(true);
    }

    public void OnRoomCleared(Room room)
    {
        room.isCleared = true;
        GameManager.Instance.roomsCleared++;
    }

    public List<Room> GetRooms() => rooms;
}
