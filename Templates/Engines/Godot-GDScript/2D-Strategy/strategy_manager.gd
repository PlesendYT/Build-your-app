extends Node

@export var map_width: int = 20
@export var map_height: int = 15
@export var tile_size: int = 32

@onready var tile_map: TileMap = $TileMap
@onready var units: Node = $Units
@onready var ui: CanvasLayer = $UI

var current_player: int = 1
var players: Dictionary = {}
var selected_unit: Node = null
var turn_count: int = 0

func _ready() -> void:
	init_players()
	spawn_starting_units()

func init_players() -> void:
	players[1] = {"gold": 100, "color": Color.BLUE, "units": []}
	players[2] = {"gold": 100, "color": Color.RED, "units": []}

func spawn_starting_units() -> void:
	spawn_unit("warrior", Vector2i(1, 1), 1)
	spawn_unit("archer", Vector2i(2, 1), 1)
	spawn_unit("warrior", Vector2i(map_width - 2, map_height - 2), 2)
	spawn_unit("archer", Vector2i(map_width - 3, map_height - 2), 2)

func spawn_unit(unit_type: String, cell: Vector2i, player_id: int) -> void:
	var unit: Node2D = preload("res://unit.tscn").instantiate()
	unit.init(unit_type, player_id)
	unit.global_position = tile_map.map_to_local(cell)
	unit.cell_position = cell
	units.add_child(unit)
	players[player_id].units.append(unit)
	tile_map.set_cell(0, cell, 1, Vector2i(player_id - 1, 0))

func start_turn() -> void:
	turn_count += 1
	update_ui()
	for unit in players[current_player].units:
		unit.has_moved = false
		unit.has_attacked = false

func end_turn() -> void:
	current_player = 2 if current_player == 1 else 1
	start_turn()

func select_unit(unit: Node) -> void:
	if unit.player_id != current_player or unit.has_moved:
		return

	selected_unit = unit
	show_movement_range(unit)
	update_ui()

func move_unit(target_cell: Vector2i) -> void:
	if selected_unit == null:
		return

	var path: Array[Vector2i] = find_path(selected_unit.cell_position, target_cell, selected_unit.move_range)
	if path.size() > 0:
		tile_map.set_cell(0, selected_unit.cell_position, -1)
		selected_unit.cell_position = target_cell
		selected_unit.global_position = tile_map.map_to_local(target_cell)
		selected_unit.has_moved = true
		tile_map.set_cell(0, target_cell, 1, Vector2i(selected_unit.player_id - 1, 0))
		clear_highlight()

func find_path(from: Vector2i, to: Vector2i, max_distance: int) -> Array[Vector2i]:
	var path: Array[Vector2i] = []
	var current: Vector2i = from
	var steps: int = 0

	while current != to and steps < max_distance:
		steps += 1
		var dx: int = clampi(to.x - current.x, -1, 1)
		var dy: int = clampi(to.y - current.y, -1, 1)
		var next: Vector2i = Vector2i(current.x + dx, current.y + dy)

		if tile_map.get_cell_source_id(0, next) == -1:
			current = next
			path.append(current)
		else:
			break

	return path

func attack_unit(attacker: Node, target: Node) -> void:
	if attacker.has_attacked or target.player_id == attacker.player_id:
		return

	var damage: int = attacker.attack - target.defense
	damage = max(1, damage)
	target.health -= damage
	attacker.has_attacked = true

	if target.health <= 0:
		destroy_unit(target)

func destroy_unit(unit: Node) -> void:
	players[unit.player_id].units.erase(unit)
	tile_map.set_cell(0, unit.cell_position, -1)
	players[unit.player_id].gold += 25
	unit.queue_free()

	if players[unit.player_id].units.size() == 0:
		game_over(3 - unit.player_id)

func show_movement_range(unit: Node) -> void:
	clear_highlight()
	for x in range(-unit.move_range, unit.move_range + 1):
		for y in range(-unit.move_range, unit.move_range + 1):
			var cell: Vector2i = unit.cell_position + Vector2i(x, y)
			if tile_map.get_cell_source_id(0, cell) == -1:
				tile_map.set_cell(1, cell, 2, Vector2i(0, 0))

func clear_highlight() -> void:
	for x in range(map_width):
		for y in range(map_height):
			tile_map.set_cell(1, Vector2i(x, y), -1)

func game_over(winner: int) -> void:
	ui.get_node("GameOverLabel").text = "Player " + str(winner) + " wins!"
	ui.get_node("GameOverLabel").show()

func update_ui() -> void:
	ui.get_node("GoldLabel").text = "Gold: " + str(players[current_player].gold)
	ui.get_node("TurnLabel").text = "Turn: " + str(turn_count)
	ui.get_node("PlayerLabel").text = "Player " + str(current_player)
