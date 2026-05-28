extends Node2D

@export var speed: float = 400.0
@export var lane: int = 0
@export var note_type: String = "normal"

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D

var is_hit: bool = false

func _physics_process(delta: float) -> void:
	if not is_hit:
		position.y += speed * delta

func hit(accuracy: String) -> void:
	is_hit = true
	animated_sprite.play(accuracy)
	await animated_sprite.animation_finished
	queue_free()

func get_lane() -> int:
	return lane

func _on_visible_on_screen_notifier_2d_screen_exited() -> void:
	if not is_hit:
		queue_free()
		get_parent().hit_note(lane, "miss")
