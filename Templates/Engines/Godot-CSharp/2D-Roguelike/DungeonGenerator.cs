using Godot;
using System;
using System.Collections.Generic;

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

    public void GenerateDungeon()
    {
        _mapData = new TileType[MapWidth, MapHeight];
        _rooms.Clear();

        for (int x = 0; x < MapWidth; x++)
            for (int y = 0; y < MapHeight; y++)
                _mapData[x, y] = TileType.Wall;

        var rng = new Random();
        for (int i = 0; i < MaxRooms; i++)
        {
            int w = rng.Next(RoomMinSize, RoomMaxSize + 1);
            int h = rng.Next(RoomMinSize, RoomMaxSize + 1);
            int x = rng.Next(1, MapWidth - w - 1);
            int y = rng.Next(1, MapHeight - h - 1);

            var newRoom = new Rect2I(x, y, w, h);
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

    private void CarveRoom(Rect2I room)
    {
        for (int x = room.Position.X; x < room.Position.X + room.Size.X; x++)
            for (int y = room.Position.Y; y < room.Position.Y + room.Size.Y; y++)
                _mapData[x, y] = TileType.Floor;
    }

    private void ConnectRooms()
    {
        var rng = new Random();
        for (int i = 0; i < _rooms.Count - 1; i++)
        {
            var centerA = _rooms[i].GetCenter();
            var centerB = _rooms[i + 1].GetCenter();

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

    private void CarveHCorridor(int x1, int x2, int y)
    {
        for (int x = Mathf.Min(x1, x2); x <= Mathf.Max(x1, x2); x++)
            if (x >= 0 && x < MapWidth && y >= 0 && y < MapHeight)
                _mapData[x, y] = TileType.Corridor;
    }

    private void CarveVCorridor(int y1, int y2, int x)
    {
        for (int y = Mathf.Min(y1, y2); y <= Mathf.Max(y1, y2); y++)
            if (x >= 0 && x < MapWidth && y >= 0 && y < MapHeight)
                _mapData[x, y] = TileType.Corridor;
    }

    private void RenderMap()
    {
        _tileMap.Clear();
        for (int x = 0; x < MapWidth; x++)
            for (int y = 0; y < MapHeight; y++)
                _tileMap.SetCell(0, new Vector2I(x, y), 0, new Vector2I((int)_mapData[x, y], 0));
    }

    public Vector2I GetRandomFloorTile()
    {
        var rng = new Random();
        var room = _rooms[rng.Next(_rooms.Count)];
        int x = rng.Next(room.Position.X, room.Position.X + room.Size.X);
        int y = rng.Next(room.Position.Y, room.Position.Y + room.Size.Y);
        return new Vector2I(x, y);
    }
}
