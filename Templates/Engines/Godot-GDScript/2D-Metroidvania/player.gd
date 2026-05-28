extends CharacterBody2D

@export var speed: float = 280.0
@export var jump_velocity: float = -400.0
@export var wall_slide_speed: float = 60.0
@export var wall_jump_velocity: Vector2 = Vector2(300, -350)
@export var gravity: float = 980.0

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var camera: Camera2D = $Camera2D

var has_double_jump: bool = false
var has_dash: bool = false
var has_wall_climb: bool = false
var health: int = 5
var max_health: int = 5
var energy: int = 100
var max_energy: int = 100
var double_jump_used: bool = false
var is_wall_sliding: bool = false

func _physics_process(delta: float) -> void:
	apply_gravity(delta)
	handle_movement()
	handle_jump()
	handle_wall_interaction()
	move_and_slide()
	update_animation()
	update_facing()

func apply_gravity(delta: float) -> void:
	if not is_on_floor():
		velocity.y += gravity * delta

func handle_movement() -> void:
	var direction: float = Input.get_axis("move_left", "move_right")
	velocity.x = direction * speed if direction != 0 else 0.0

func handle_jump() -> void:
	if is_on_floor():
		double_jump_used = false

	if Input.is_action_just_pressed("jump"):
		if is_on_floor():
			velocity.y = jump_velocity
		elif has_double_jump and not double_jump_used:
			velocity.y = jump_velocity * 0.9
			double_jump_used = true
		elif is_wall_sliding:
			velocity = wall_jump_velocity
			animated_sprite.flip_h = not animated_sprite.flip_h

func handle_wall_interaction() -> void:
	is_wall_sliding = false
	if has_wall_climb and not is_on_floor() and is_on_wall():
		is_wall_sliding = true
		velocity.y = min(velocity.y, wall_slide_speed)

func update_animation() -> void:
	if not is_on_floor():
		animated_sprite.play("jump")
	elif is_wall_sliding:
		animated_sprite.play("wall_slide")
	elif velocity.x != 0:
		animated_sprite.play("run")
	else:
		animated_sprite.play("idle")

func update_facing() -> void:
	if velocity.x != 0:
		animated_sprite.flip_h = velocity.x < 0

func take_damage(amount: int) -> void:
	health -= amount
	if health <= 0:
		die()

func heal(amount: int) -> void:
	health = min(health + amount, max_health)

func die() -> void:
	position = Vector2.ZERO
	health = max_health
