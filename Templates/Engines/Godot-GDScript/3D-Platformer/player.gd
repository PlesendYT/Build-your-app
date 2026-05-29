# 3D platformer player controller with double-jump, checkpoint system, and coin collection
extends CharacterBody3D

@export var speed: float = 8.0
@export var jump_velocity: float = 8.0
@export var double_jump_velocity: float = 7.0
@export var gravity: float = 18.0
@export var acceleration: float = 0.2

@onready var camera_pivot: Node3D = $CameraPivot
@onready var camera: Camera3D = $CameraPivot/Camera3D
@onready var animated_sprite: AnimatedSprite3D = $AnimatedSprite3D

var has_double_jump: bool = true
var jump_count: int = 0
var collected_coins: int = 0
var current_checkpoint: Vector3 = Vector3.ZERO

func _ready() -> void:
	current_checkpoint = global_position

func _physics_process(delta: float) -> void:
	velocity.y -= gravity * delta
	handle_movement()
	handle_jump()
	move_and_slide()

	if is_on_floor():
		jump_count = 0

	update_animation()

func handle_movement() -> void:
	var input_dir: Vector2 = Input.get_vector("move_left", "move_right", "move_forward", "move_backward")
	var direction: Vector3 = (camera_pivot.transform.basis * Vector3(input_dir.x, 0, -input_dir.y)).normalized()

	if direction:
		velocity.x = lerp(velocity.x, direction.x * speed, acceleration)
		velocity.z = lerp(velocity.z, direction.z * speed, acceleration)
	else:
		velocity.x = lerp(velocity.x, 0.0, acceleration)
		velocity.z = lerp(velocity.z, 0.0, acceleration)

func handle_jump() -> void:
	if Input.is_action_just_pressed("jump"):
		if is_on_floor():
			velocity.y = jump_velocity
			jump_count = 1
		elif has_double_jump and jump_count < 2:
			velocity.y = double_jump_velocity
			jump_count += 1

func update_animation() -> void:
	if not is_on_floor():
		animated_sprite.play("jump")
	elif velocity.length() > 0.5:
		animated_sprite.play("run")
	else:
		animated_sprite.play("idle")

	if velocity.length() > 0.5:
		var look_direction: Vector2 = Vector2(velocity.x, velocity.z)
		animated_sprite.flip_h = look_direction.x < 0

func collect_coin() -> void:
	collected_coins += 1

func set_checkpoint(position: Vector3) -> void:
	current_checkpoint = position

func die() -> void:
	global_position = current_checkpoint
	velocity = Vector3.ZERO

func _on_hurtbox_area_entered(area: Area3D) -> void:
	if area.is_in_group("hazard"):
		die()
