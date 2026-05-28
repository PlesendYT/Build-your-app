extends Node2D

@export var starting_gold: int = 100
@export var starting_lives: int = 20
@export var wave_data: Array[Dictionary] = []

@onready var gold_label: Label = $UI/GoldLabel
@onready var lives_label: Label = $UI/LivesLabel
@onready var wave_label: Label = $UI/WaveLabel
@onready var spawn_path: Path2D = $SpawnPath
@onready var spawn_timer: Timer = $SpawnTimer

var gold: int
var lives: int
var current_wave: int = 0
var enemies_in_wave: int = 0
var enemies_spawned: int = 0
var is_wave_active: bool = false
var selected_tower_type: String = "basic"

func _ready() -> void:
	gold = starting_gold
	lives = starting_lives
	update_ui()

func start_wave() -> void:
	if is_wave_active:
		return
	if current_wave >= wave_data.size():
		return

	var wave: Dictionary = wave_data[current_wave]
	enemies_in_wave = wave.get("count", 10)
	enemies_spawned = 0
	is_wave_active = true
	spawn_timer.start(wave.get("spawn_interval", 0.5))

func _on_spawn_timer_timeout() -> void:
	if enemies_spawned >= enemies_in_wave:
		spawn_timer.stop()
		check_wave_complete()
		return

	var wave: Dictionary = wave_data[current_wave]
	var enemy_scene: PackedScene = wave.get("enemy_scene")
	var enemy: Node2D = enemy_scene.instantiate()
	enemy.add_to_group("enemies")
	add_child(enemy)
	enemies_spawned += 1

func check_wave_complete() -> void:
	if get_tree().get_nodes_in_group("enemies").size() == 0:
		is_wave_active = false
		current_wave += 1
		gold += 50
		update_ui()

func select_tower(tower_type: String) -> void:
	selected_tower_type = tower_type

func place_tower(position: Vector2) -> void:
	var tower_data: Dictionary = get_tower_data(selected_tower_type)
	if gold < tower_data.get("cost", 50):
		return

	gold -= tower_data.get("cost", 50)
	var tower_scene: PackedScene = tower_data.get("scene")
	var tower: Node2D = tower_scene.instantiate()
	tower.global_position = position
	tower.tower_type = selected_tower_type
	add_child(tower)
	update_ui()

func get_tower_data(tower_type: String) -> Dictionary:
	match tower_type:
		"basic":
			return {"cost": 50, "damage": 10, "range": 150, "rate": 1.0}
		"sniper":
			return {"cost": 100, "damage": 40, "range": 300, "rate": 2.0}
		"rapid":
			return {"cost": 75, "damage": 5, "range": 120, "rate": 0.2}
		"aoe":
			return {"cost": 150, "damage": 15, "range": 100, "rate": 1.5}
	return {"cost": 50, "damage": 10, "range": 150, "rate": 1.0}

func enemy_reached_end(enemy: Node) -> void:
	lives -= enemy.damage_to_base
	enemy.queue_free()
	if lives <= 0:
		game_over()
	update_ui()

func enemy_killed(gold_value: int) -> void:
	gold += gold_value
	update_ui()

func game_over() -> void:
	get_tree().paused = true
	$UI/GameOverScreen.show()

func update_ui() -> void:
	gold_label.text = "Gold: " + str(gold)
	lives_label.text = "Lives: " + str(lives)
	wave_label.text = "Wave: " + str(current_wave + 1)
