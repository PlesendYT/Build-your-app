# Roguelike enemy with turn-based AI, pathfinding, combat, and XP reward on death
extends Node2D

@export var health: int = 5
@export var attack_power: int = 2
@export var defense: int = 0
@export var xp_value: int = 5
@export var vision_range: int = 8

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D

var tile_map: TileMap
var player: Node = null
var is_alive: bool = true

func _ready() -> void:
	tile_map = get_parent().get_node("TileMap")
	player = get_parent().get_node("Player")
	add_to_group("enemies")

func take_turn() -> void:
	if not is_alive or player == null:
		return

	var player_cell: Vector2i = tile_map.local_to_map(player.global_position)
	var my_cell: Vector2i = tile_map.local_to_map(global_position)
	var distance: int = abs(player_cell.x - my_cell.x) + abs(player_cell.y - my_cell.y)

	if distance <= 1:
		player.take_damage(attack_power)
	elif distance <= vision_range:
		move_toward(player_cell)

func move_toward(target_cell: Vector2i) -> void:
	var my_cell: Vector2i = tile_map.local_to_map(global_position)
	var path: Array[Vector2i] = get_path(my_cell, target_cell)

	if path.size() > 1:
		var next_cell: Vector2i = path[1]
		var target_pos: Vector2 = tile_map.map_to_local(next_cell)
		global_position = target_pos

func get_path(from: Vector2i, to: Vector2i) -> Array[Vector2i]:
	var path: Array[Vector2i] = []
	var current: Vector2i = from
	var max_steps: int = 50

	while current != to and max_steps > 0:
		max_steps -= 1
		var dx: int = sign(to.x - current.x)
		var dy: int = sign(to.y - current.y)

		if dx != 0:
			var next_x: Vector2i = Vector2i(current.x + dx, current.y)
			if tile_map.get_cell_tile_data(0, next_x) != null:
				current = next_x
				path.append(current)
				continue

		if dy != 0:
			var next_y: Vector2i = Vector2i(current.x, current.y + dy)
			if tile_map.get_cell_tile_data(0, next_y) != null:
				current = next_y
				path.append(current)
				continue

		break

	return path

func take_damage(amount: int) -> void:
	health -= amount
	if health <= 0:
		die()

func die() -> void:
	is_alive = false
	player.add_xp(xp_value)
	animated_sprite.play("death")
	await animated_sprite.animation_finished
	queue_free()
