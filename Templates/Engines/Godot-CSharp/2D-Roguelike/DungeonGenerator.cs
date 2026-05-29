// File: DungeonGenerator.cs
// Procedurally generates a 2D dungeon with rooms, corridors, and walls using a TileMap.

using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Generates a tile-based dungeon by carving random non-overlapping rooms and connecting them
/// with L-shaped corridors. Exposes GetRandomFloorTile for spawning entities.
/// </summary>
public partial class DungeonGenerator : Node2D
{
    [Export] public int MapWidth = 40;
    [Export] public int MapHeight = 30;
    [Export] public int MaxRooms = 10;
    [Export] public int RoomMinSize = 5;
    [Export] public int RoomMaxSize = 10;

    private TileMap _tileMap;
    private enum TileType { Wall, Floor, Corridor }
    private TileType[,] _mapData;
    private List<Rect2I> _rooms = new();

    public override void _Ready()
    {
        _tileMap = GetNode<TileMap>("TileMap");
        GenerateDungeon();
    }

    /// <summary>
    /// Runs the full dungeon generation algorithm: fill with walls, carve rooms, connect them, render.
    /// </summary>
    public void GenerateDungeon()
    {
        _mapData = new TileType[MapWidth, MapHeight];
        _rooms.Clear();

        // Fill the entire map with walls
        for (int x = 0; x < MapWidth; x++)
            for (int y = 0; y < MapHeight; y++)
                _mapData[x, y] = TileType.Wall;

        // Try to place rooms at random positions
        var rng = new Random();
        for (int i = 0; i < MaxRooms; i++)
        {
            int w = rng.Next(RoomMinSize, RoomMaxSize + 1);
            int h = rng.Next(RoomMinSize, RoomMaxSize + 1);
            int x = rng.Next(1, MapWidth - w - 1);
            int y = rng.Next(1, MapHeight - h - 1);

            var newRoom = new Rect2I(x, y, w, h);
            // Check for overlap with existing rooms (with 1-tile padding)
            bool overlaps = false;
            foreach (var room in _rooms)
            {
                if (newRoom.Intersects(room.Grow(1)))
                {
                    overlaps = true;
                    break;
                }
            }

            if (!overlaps)
            {
                CarveRoom(newRoom);
                _rooms.Add(newRoom);
            }
        }

        ConnectRooms();
        RenderMap();
    }

    /// <summary>
    /// Sets all tiles within the room rectangle to Floor.
    /// </summary>
    private void CarveRoom(Rect2I room)
    {
        for (int x = room.Position.X; x < room.Position.X + room.Size.X; x++)
            for (int y = room.Position.Y; y < room.Position.Y + room.Size.Y; y++)
                _mapData[x, y] = TileType.Floor;
    }

    /// <summary>
    /// Connects each consecutive pair of rooms with an L-shaped corridor (horizontal then vertical or vice versa).
    /// </summary>
    private void ConnectRooms()
    {
        var rng = new Random();
        for (int i = 0; i < _rooms.Count - 1; i++)
        {
            var centerA = _rooms[i].GetCenter();
            var centerB = _rooms[i + 1].GetCenter();

            // Randomly choose horizontal-first or vertical-first L-shape
            if (rng.NextDouble() > 0.5)
            {
                CarveHCorridor(centerA.X, centerB.X, centerA.Y);
                CarveVCorridor(centerA.Y, centerB.Y, centerB.X);
            }
            else
            {
                CarveVCorridor(centerA.Y, centerB.Y, centerA.X);
                CarveHCorridor(centerA.X, centerB.X, centerB.Y);
            }
        }
    }

    /// <summary>
    /// Carves a horizontal corridor at row y between columns x1 and x2.
    /// </summary>
    private void CarveHCorridor(int x1, int x2, int y)
    {
        for (int x = Mathf.Min(x1, x2); x <= Mathf.Max(x1, x2); x++)
            if (x >= 0 && x < MapWidth && y >= 0 && y < MapHeight)
                _mapData[x, y] = TileType.Corridor;
    }

    /// <summary>
    /// Carves a vertical corridor at column x between rows y1 and y2.
    /// </summary>
    private void CarveVCorridor(int y1, int y2, int x)
    {
        for (int y = Mathf.Min(y1, y2); y <= Mathf.Max(y1, y2); y++)
            if (x >= 0 && x < MapWidth && y >= 0 && y < MapHeight)
                _mapData[x, y] = TileType.Corridor;
    }

    /// <summary>
    /// Clears and redraws the TileMap based on the generated map data.
    /// </summary>
    private void RenderMap()
    {
        _tileMap.Clear();
        for (int x = 0; x < MapWidth; x++)
            for (int y = 0; y < MapHeight; y++)
                _tileMap.SetCell(0, new Vector2I(x, y), 0, new Vector2I((int)_mapData[x, y], 0));
    }

    /// <summary>
    /// Returns a random floor tile position from one of the generated rooms (for spawning entities).
    /// </summary>
    public Vector2I GetRandomFloorTile()
    {
        var rng = new Random();
        var room = _rooms[rng.Next(_rooms.Count)];
        int x = rng.Next(room.Position.X, room.Position.X + room.Size.X);
        int y = rng.Next(room.Position.Y, room.Position.Y + room.Size.Y);
        return new Vector2I(x, y);
    }
}
