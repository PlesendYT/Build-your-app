# Beat-em-up enemy with aggro detection, attack, health bar, and death sequence
extends CharacterBody2D

@export var health: int = 30
@export var damage: int = 5
@export var aggro_range: float = 200.0
@export var enemy_type: String = "grunt"

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var health_bar: ProgressBar = $HealthBar
@onready var detection_area: Area2D = $DetectionArea

var player: Node = null
var is_dead: bool = false

func _ready() -> void:
	health_bar.max_value = health
	health_bar.value = health
	add_to_group("enemies")

func _physics_process(delta: float) -> void:
	if is_dead or player == null:
		return

	var direction_to_player: Vector2 = (player.global_position - global_position).normalized()
	var distance: float = global_position.distance_to(player.global_position)

	if distance < 50:
		attack_player()
	elif distance < aggro_range:
		velocity.x = direction_to_player.x * 100
		animated_sprite.play("walk")
	else:
		velocity.x = 0
		animated_sprite.play("idle")

	animated_sprite.flip_h = velocity.x < 0
	move_and_slide()

func attack_player() -> void:
	animated_sprite.play("attack")
	player.take_damage(damage)

func take_damage(amount: int) -> void:
	health -= amount
	health_bar.value = health
	if health <= 0:
		die()

func die() -> void:
	is_dead = true
	set_physics_process(false)
	$CollisionShape2D.set_deferred("disabled", true)
	animated_sprite.play("death")
	await animated_sprite.animation_finished
	queue_free()

func _on_detection_area_body_entered(body: Node2D) -> void:
	if body.is_in_group("player"):
		player = body
