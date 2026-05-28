extends CharacterBody2D

@export var speed: float = 150.0
@export var health: int = 2
@export var score_value: int = 100
@export var shoot_interval: float = 2.0
@export var bullet_scene: PackedScene

@onready var shoot_timer: Timer = $ShootTimer
@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D

var movement_pattern: int = 0
var start_position: Vector2

func _ready() -> void:
	start_position = position
	shoot_timer.timeout.connect(_shoot)
	shoot_timer.start(shoot_interval)

func _physics_process(delta: float) -> void:
	match movement_pattern:
		0:
			velocity = Vector2(-speed, 0)
		1:
			velocity = Vector2(-speed, sin(position.y * 0.05) * 100)
		2:
			velocity = Vector2(-speed, 0)
			if position.distance_to(start_position) > 200:
				start_position = position
				movement_pattern = randi() % 3

	move_and_slide()

func _shoot() -> void:
	if bullet_scene:
		var bullet: Area2D = bullet_scene.instantiate()
		bullet.global_position = $GunPoint.global_position
		bullet.direction = Vector2.LEFT
		get_parent().add_child(bullet)

func take_damage(amount: int) -> void:
	health -= amount
	if health <= 0:
		die()

func die() -> void:
	Events.emit_signal("enemy_killed", score_value)
	animated_sprite.play("explosion")
	set_physics_process(false)
	$CollisionShape2D.set_deferred("disabled", true)
	await animated_sprite.animation_finished
	queue_free()
