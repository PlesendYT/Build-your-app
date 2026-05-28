extends AnimatableBody3D

@export var move_to: Vector3 = Vector3(10, 0, 0)
@export var move_speed: float = 2.0
@export var pause_time: float = 1.0

@onready var start_position: Vector3 = position

var moving_forward: bool = true
var is_paused: bool = false

func _physics_process(delta: float) -> void:
	if is_paused:
		return

	var target: Vector3 = start_position + move_to if moving_forward else start_position
	var direction: Vector3 = (target - position).normalized()
	global_position += direction * move_speed * delta

	if position.distance_to(target) < 0.1:
		moving_forward = not moving_forward
		is_paused = true
		await get_tree().create_timer(pause_time).timeout
		is_paused = false
