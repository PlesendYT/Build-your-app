# Roguelike dungeon generator using BSP-like room placement with corridors
extends Node2D

@export var map_width: int = 40
@export var map_height: int = 30
@export var tile_size: int = 16
@export var room_max_size: int = 10
@export var room_min_size: int = 5
@export var max_rooms: int = 10

@onready var tile_map: TileMap = $TileMap

enum TileType { WALL = 0, FLOOR = 1, CORRIDOR = 2 }

var map_data: Array[Array] = []
var rooms: Array[Rect2i] = []

func _ready() -> void:
	generate_dungeon()

func generate_dungeon() -> void:
	initialize_map()
	place_rooms()
	connect_rooms()
	render_map()

func initialize_map() -> void:
	map_data.clear()
	rooms.clear()
	for x in range(map_width):
		map_data.append([])
		for y in range(map_height):
			map_data[x].append(TileType.WALL)

func place_rooms() -> void:
	for i in range(max_rooms):
		var w: int = randi_range(room_min_size, room_max_size)
		var h: int = randi_range(room_min_size, room_max_size)
		var x: int = randi_range(1, map_width - w - 1)
		var y: int = randi_range(1, map_height - h - 1)
		var new_room: Rect2i = Rect2i(x, y, w, h)

		var overlaps: bool = false
		for room in rooms:
			if new_room.intersects(room.grow(1)):
				overlaps = true
				break

		if not overlaps:
			carve_room(new_room)
			rooms.append(new_room)

func carve_room(room: Rect2i) -> void:
	for x in range(room.position.x, room.position.x + room.size.x):
		for y in range(room.position.y, room.position.y + room.size.y):
			map_data[x][y] = TileType.FLOOR

func connect_rooms() -> void:
	for i in range(rooms.size() - 1):
		var center_a: Vector2i = rooms[i].get_center()
		var center_b: Vector2i = rooms[i + 1].get_center()

		if randf() > 0.5:
			carve_h_corridor(center_a.x, center_b.x, center_a.y)
			carve_v_corridor(center_a.y, center_b.y, center_b.x)
		else:
			carve_v_corridor(center_a.y, center_b.y, center_a.x)
			carve_h_corridor(center_a.x, center_b.x, center_b.y)

func carve_h_corridor(x1: int, x2: int, y: int) -> void:
	for x in range(min(x1, x2), max(x1, x2) + 1):
		if x >= 0 and x < map_width and y >= 0 and y < map_height:
			map_data[x][y] = TileType.CORRIDOR

func carve_v_corridor(y1: int, y2: int, x: int) -> void:
	for y in range(min(y1, y2), max(y1, y2) + 1):
		if x >= 0 and x < map_width and y >= 0 and y < map_height:
			map_data[x][y] = TileType.CORRIDOR

func render_map() -> void:
	tile_map.clear()
	for x in range(map_width):
		for y in range(map_height):
			var tile: int = map_data[x][y]
			tile_map.set_cell(0, Vector2i(x, y), 0, Vector2i(tile, 0))

func get_random_floor_tile() -> Vector2i:
	var room: Rect2i = rooms[randi() % rooms.size()]
	var x: int = randi_range(room.position.x, room.position.x + room.size.x - 1)
	var y: int = randi_range(room.position.y, room.position.y + room.size.y - 1)
	return Vector2i(x, y)
