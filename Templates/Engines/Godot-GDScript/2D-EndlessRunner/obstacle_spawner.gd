# Endless runner obstacle spawner with random intervals and difficulty scaling
extends Node2D

@export var obstacle_scenes: Array[PackedScene] = []
@export var spawn_interval_min: float = 0.8
@export var spawn_interval_max: float = 2.0
@export var obstacle_speed: float = 400.0

@onready var spawn_timer: Timer = $SpawnTimer
@onready var spawn_position: Marker2D = $SpawnPosition

var current_speed: float

func _ready() -> void:
	current_speed = obstacle_speed
	reset_spawn_timer()
	Events.game_over.connect(_on_game_over)

func reset_spawn_timer() -> void:
	var interval: float = randf_range(spawn_interval_min, spawn_interval_max)
	spawn_timer.start(interval)

func _on_spawn_timer_timeout() -> void:
	if obstacle_scenes.size() > 0:
		var scene: PackedScene = obstacle_scenes[randi() % obstacle_scenes.size()]
		var obstacle: Node2D = scene.instantiate()
		obstacle.global_position = spawn_position.global_position
		obstacle.speed = current_speed
		add_child(obstacle)

	reset_spawn_timer()

func increase_difficulty() -> void:
	current_speed += 10
	if spawn_interval_min > 0.3:
		spawn_interval_min -= 0.02
	if spawn_interval_max > 0.6:
		spawn_interval_max -= 0.03

func _on_game_over(final_score: int) -> void:
	spawn_timer.stop()
