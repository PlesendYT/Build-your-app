extends Node2D

@export var move_delay: float = 0.15

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var turn_timer: Timer = $TurnTimer

var tile_map: TileMap
var can_move: bool = true
var health: int = 10
var max_health: int = 10
var attack: int = 2
var defense: int = 1
var level: int = 1
var xp: int = 0
var xp_to_next: int = 10

func _ready() -> void:
	tile_map = get_parent().get_node("TileMap")
	global_position = tile_map.map_to_local(Vector2i(5, 5))
	turn_timer.timeout.connect(_on_turn_end)

func _unhandled_input(event: InputEvent) -> void:
	if not can_move:
		return

	var direction: Vector2i = Vector2i.ZERO
	if event.is_action_pressed("move_up"):
		direction = Vector2i(0, -1)
	elif event.is_action_pressed("move_down"):
		direction = Vector2i(0, 1)
	elif event.is_action_pressed("move_left"):
		direction = Vector2i(-1, 0)
	elif event.is_action_pressed("move_right"):
		direction = Vector2i(1, 0)

	if direction != Vector2i.ZERO:
		try_move(direction)

func try_move(direction: Vector2i) -> void:
	var target_cell: Vector2i = tile_map.local_to_map(global_position) + direction
	var tile_data: TileData = tile_map.get_cell_tile_data(0, target_cell)

	if tile_data != null and tile_data.get_custom_data("walkable"):
		can_move = false
		var target_pos: Vector2 = tile_map.map_to_local(target_cell)
		var tween: Tween = create_tween()
		tween.tween_property(self, "global_position", target_pos, move_delay)
		await tween.finished
		advance_turn()
	else:
		check_enemy_at(target_cell)

func check_enemy_at(cell: Vector2i) -> void:
	var space_state: PhysicsDirectSpaceState2D = get_world_2d().direct_space_state
	var query: PhysicsPointQueryParameters2D = PhysicsPointQueryParameters2D.new()
	query.position = tile_map.map_to_local(cell)
	var results: Array[Dictionary] = space_state.intersect_point(query)

	for result in results:
		var collider: Node = result.collider
		if collider.is_in_group("enemy"):
			attack_enemy(collider)
			advance_turn()
			return

func attack_enemy(enemy: Node) -> void:
	var damage: int = max(1, attack - enemy.defense)
	enemy.take_damage(damage)

func advance_turn() -> void:
	can_move = false
	get_tree().call_group("enemies", "take_turn")
	turn_timer.start(move_delay)

func _on_turn_end() -> void:
	can_move = true

func take_damage(amount: int) -> void:
	var actual_damage: int = max(1, amount - defense)
	health -= actual_damage
	if health <= 0:
		die()

func add_xp(amount: int) -> void:
	xp += amount
	while xp >= xp_to_next:
		level_up()

func level_up() -> void:
	xp -= xp_to_next
	level += 1
	xp_to_next = floor(xp_to_next * 1.5)
	max_health += 2
	health = max_health
	attack += 1

func die() -> void:
	get_tree().reload_current_scene()
