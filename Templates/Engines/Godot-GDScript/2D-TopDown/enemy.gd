# Top-down enemy with patrol waypoints, chase AI, and directional animation
extends CharacterBody2D

@export var patrol_points: Array[Vector2] = []
@export var move_speed: float = 80.0
@export var damage: int = 1

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var detection_area: Area2D = $DetectionArea

var current_point_index: int = 0
var chasing: bool = false
var target: Node2D = null

func _physics_process(delta: float) -> void:
	if chasing and is_instance_valid(target):
		chase_target()
	else:
		patrol()

	move_and_slide()
	update_animation()

func patrol() -> void:
	if patrol_points.size() == 0:
		return

	var target_pos: Vector2 = patrol_points[current_point_index]
	var direction: Vector2 = (target_pos - position).normalized()
	velocity = direction * move_speed

	if position.distance_to(target_pos) < 10.0:
		current_point_index = (current_point_index + 1) % patrol_points.size()

func chase_target() -> void:
	var direction: Vector2 = (target.position - position).normalized()
	velocity = direction * (move_speed * 1.5)

func update_animation() -> void:
	animated_sprite.play("walk")
	if abs(velocity.x) > abs(velocity.y):
		animated_sprite.flip_h = velocity.x < 0
		animated_sprite.animation = "walk_side"
	elif velocity.y < 0:
		animated_sprite.animation = "walk_up"
	else:
		animated_sprite.animation = "walk_down"

func _on_detection_area_body_entered(body: Node2D) -> void:
	if body.is_in_group("player"):
		chasing = true
		target = body

func _on_detection_area_body_exited(body: Node2D) -> void:
	if body == target:
		chasing = false
		target = null

func _on_hurtbox_area_entered(area: Area2D) -> void:
	if area.is_in_group("player_attack"):
		queue_free()
