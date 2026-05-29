# Platformer player with double-jump, coyote time, acceleration/friction, and jump sound
extends CharacterBody2D

@export var speed: float = 300.0
@export var jump_velocity: float = -400.0
@export var double_jump_velocity: float = -350.0
@export var acceleration: float = 0.2
@export var friction: float = 0.1
@export var gravity: float = 980.0

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var jump_sound: AudioStreamPlayer2D = $JumpSound
@onready var coyote_timer: Timer = $CoyoteTimer

var has_double_jump: bool = true
var jumps_remaining: int = 0

func _ready() -> void:
	jumps_remaining = 2

func _physics_process(delta: float) -> void:
	apply_gravity(delta)
	handle_movement()
	handle_jump()
	move_and_slide()
	update_animation()
	update_facing_direction()

func apply_gravity(delta: float) -> void:
	if not is_on_floor():
		velocity.y += gravity * delta

func handle_movement() -> void:
	var direction: float = Input.get_axis("move_left", "move_right")
	if direction != 0:
		velocity.x = lerp(velocity.x, direction * speed, acceleration)
	else:
		velocity.x = lerp(velocity.x, 0.0, friction)

func handle_jump() -> void:
	if is_on_floor():
		jumps_remaining = 2

	if Input.is_action_just_pressed("jump"):
		if is_on_floor() or coyote_timer.time_left > 0:
			velocity.y = jump_velocity
			jumps_remaining -= 1
			jump_sound.play()
		elif jumps_remaining > 0:
			velocity.y = double_jump_velocity
			jumps_remaining -= 1
			jump_sound.play()

	if Input.is_action_just_released("jump") and velocity.y < 0:
		velocity.y *= 0.5

func update_animation() -> void:
	if not is_on_floor():
		animated_sprite.play("jump")
	elif velocity.x != 0:
		animated_sprite.play("run")
	else:
		animated_sprite.play("idle")

func update_facing_direction() -> void:
	if velocity.x > 0:
		animated_sprite.flip_h = false
	elif velocity.x < 0:
		animated_sprite.flip_h = true
