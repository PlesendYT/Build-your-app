extends Area2D

@export var required_ability: String = "double_jump"
@export var gate_id: String = "gate_1"
@export var unlocked_icon: Texture2D

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var collision_shape: CollisionShape2D = $CollisionShape2D
@onready var label: Label = $Label

var is_unlocked: bool = false

func _ready() -> void:
	if is_unlocked:
		open_gate()
	else:
		label.text = get_ability_name()

func _on_body_entered(body: Node2D) -> void:
	if body.is_in_group("player") and not is_unlocked:
		if has_ability(body):
			open_gate()
			body.show_notification(get_ability_name() + " used!")
		else:
			body.show_notification("Need: " + get_ability_name())

func has_ability(player: Node) -> bool:
	match required_ability:
		"double_jump":
			return player.has_double_jump
		"dash":
			return player.has_dash
		"wall_climb":
			return player.has_wall_climb
	return false

func get_ability_name() -> String:
	match required_ability:
		"double_jump":
			return "Double Jump"
		"dash":
			return "Dash"
		"wall_climb":
			return "Wall Climb"
	return required_ability

func open_gate() -> void:
	is_unlocked = true
	collision_shape.set_deferred("disabled", true)
	animated_sprite.play("open")
	label.hide()
