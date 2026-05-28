extends CharacterBody2D

@export var jump_velocity: float = -500.0
@export var gravity: float = 1200.0
@export var slide_duration: float = 0.8

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var slide_timer: Timer = $SlideTimer
@onready var collision_shape: CollisionShape2D = $CollisionShape2D
@onready var slide_collision: CollisionShape2D = $SlideCollision

var is_sliding: bool = false
var is_dead: bool = false
var score: int = 0

func _ready() -> void:
	slide_timer.timeout.connect(_on_slide_end)

func _physics_process(delta: float) -> void:
	if is_dead:
		return

	velocity.y += gravity * delta

	if Input.is_action_just_pressed("jump") and is_on_floor():
		velocity.y = jump_velocity
		animated_sprite.play("jump")

	if Input.is_action_just_pressed("slide") and is_on_floor() and not is_sliding:
		start_slide()

	move_and_slide()

	if is_on_floor() and not is_sliding:
		animated_sprite.play("run")

	update_score()

func start_slide() -> void:
	is_sliding = true
	collision_shape.set_deferred("disabled", true)
	slide_collision.set_deferred("disabled", false)
	animated_sprite.play("slide")
	slide_timer.start(slide_duration)

func _on_slide_end() -> void:
	is_sliding = false
	collision_shape.set_deferred("disabled", false)
	slide_collision.set_deferred("disabled", true)

func die() -> void:
	is_dead = true
	animated_sprite.play("death")
	set_physics_process(false)
	Events.emit_signal("game_over", score)

func update_score() -> void:
	score += 1
	Events.emit_signal("score_updated", score)

func _on_hurtbox_area_entered(area: Area2D) -> void:
	if not is_dead:
		die()
