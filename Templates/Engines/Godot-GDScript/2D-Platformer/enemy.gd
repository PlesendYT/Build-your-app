# Platformer enemy that patrols back and forth with edge detection
extends CharacterBody2D

@export var patrol_speed: float = 100.0
@export var patrol_distance: float = 200.0
@export var damage: int = 1

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var patrol_ray: RayCast2D = $PatrolRay

var start_position: Vector2
var moving_right: bool = true

func _ready() -> void:
	start_position = position

func _physics_process(delta: float) -> void:
	handle_patrol()
	move_and_slide()
	update_animation()

func handle_patrol() -> void:
	var distance_traveled: float = abs(position.x - start_position.x)

	if distance_traveled >= patrol_distance:
		moving_right = not moving_right

	if not patrol_ray.is_colliding():
		moving_right = not moving_right

	velocity.x = patrol_speed if moving_right else -patrol_speed

func update_animation() -> void:
	animated_sprite.play("walk")
	animated_sprite.flip_h = not moving_right

func _on_hurtbox_area_entered(area: Area2D) -> void:
	if area.is_in_group("player_attack"):
		queue_free()
