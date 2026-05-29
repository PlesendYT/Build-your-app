# Shoot-em-up player with movement, shooting, weapon levels, lives, and score tracking
extends CharacterBody2D

@export var speed: float = 350.0
@export var fire_rate: float = 0.15
@export var bullet_scene: PackedScene

@onready var gun_point: Marker2D = $GunPoint
@onready var fire_timer: Timer = $FireTimer
@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D

var can_fire: bool = true
var weapon_level: int = 1
var lives: int = 3
var score: int = 0

func _ready() -> void:
	fire_timer.timeout.connect(_on_fire_timer_timeout)

func _physics_process(delta: float) -> void:
	var direction: Vector2 = Vector2(
		Input.get_axis("move_left", "move_right"),
		Input.get_axis("move_up", "move_down")
	)

	velocity = direction * speed
	move_and_slide()
	clamp_position()

	if Input.is_action_pressed("shoot") and can_fire:
		shoot()

func clamp_position() -> void:
	global_position = global_position.clamp(
		Vector2(0, 0),
		Vector2(get_viewport().get_visible_rect().size.x, get_viewport().get_visible_rect().size.y)
	)

func shoot() -> void:
	can_fire = false
	fire_timer.start(fire_rate)
	animated_sprite.play("shoot")

	for i in range(weapon_level):
		var bullet: Area2D = bullet_scene.instantiate()
		bullet.global_position = gun_point.global_position
		bullet.direction = Vector2.RIGHT
		if i == 0:
			bullet.global_position.y -= 5
		elif i == 2:
			bullet.global_position.y += 5
		get_parent().add_child(bullet)

func _on_fire_timer_timeout() -> void:
	can_fire = true

func take_damage() -> void:
	lives -= 1
	animated_sprite.play("hit")
	if lives <= 0:
		queue_free()
		get_tree().call_group("game_manager", "game_over")

func add_score(points: int) -> void:
	score += points
