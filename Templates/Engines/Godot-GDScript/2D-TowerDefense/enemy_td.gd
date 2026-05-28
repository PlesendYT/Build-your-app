extends PathFollow2D

@export var health: int = 10
@export var speed: float = 100.0
@export var damage_to_base: int = 1
@export var gold_value: int = 10

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var health_bar: ProgressBar = $HealthBar

func _ready() -> void:
	health_bar.max_value = health
	health_bar.value = health
	add_to_group("enemies")

func _physics_process(delta: float) -> void:
	progress += speed * delta

	if progress_ratio >= 1.0:
		reached_end()

func take_damage(amount: int) -> void:
	health -= amount
	health_bar.value = health
	if health <= 0:
		die()

func die() -> void:
	get_tree().call_group("game_manager", "enemy_killed", gold_value)
	queue_free()

func reached_end() -> void:
	get_tree().call_group("game_manager", "enemy_reached_end", self)
