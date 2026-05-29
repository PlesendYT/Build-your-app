# Turn-based strategy unit with type-specific stats, movement, and combat
extends Node2D

@export var unit_type: String = "warrior"
@export var player_id: int = 1
@export var move_range: int = 3
@export var attack: int = 5
@export var defense: int = 2
@export var health: int = 10

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var health_bar: ProgressBar = $HealthBar

var cell_position: Vector2i
var has_moved: bool = false
var has_attacked: bool = false

func init(type: String, pid: int) -> void:
	unit_type = type
	player_id = pid
	match type:
		"warrior":
			attack = 6
			defense = 3
			health = 12
			move_range = 2
		"archer":
			attack = 4
			defense = 1
			health = 8
			move_range = 3
		"knight":
			attack = 7
			defense = 4
			health = 15
			move_range = 4

	health_bar.max_value = health
	health_bar.value = health
	update_appearance()

func update_appearance() -> void:
	if player_id == 1:
		animated_sprite.modulate = Color(0.2, 0.4, 1.0)
	else:
		animated_sprite.modulate = Color(1.0, 0.2, 0.2)
	animated_sprite.play(unit_type)

func _on_unit_selected() -> void:
	var game_manager: Node = get_parent().get_parent()
	game_manager.select_unit(self)

func take_damage(amount: int) -> void:
	health -= amount
	health_bar.value = health
