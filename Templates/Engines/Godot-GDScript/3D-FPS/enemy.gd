extends CharacterBody3D

@export var health: int = 50
@export var move_speed: float = 3.0
@export var attack_range: float = 15.0
@export var attack_damage: int = 10
@export var attack_cooldown: float = 2.0

@onready var animated_sprite: AnimatedSprite3D = $AnimatedSprite3D
@onready var detection_area: Area3D = $DetectionArea
@onready var attack_timer: Timer = $AttackTimer
@onready var muzzle: Node3D = $Muzzle

var player: Node = null
var can_attack: bool = true
var is_alive: bool = true

func _ready() -> void:
	add_to_group("enemies")

func _physics_process(delta: float) -> void:
	if not is_alive or player == null:
		return

	var distance: float = global_position.distance_to(player.global_position)

	if distance <= attack_range and can_attack:
		attack()
	elif distance > attack_range:
		move_toward_player()

	if player.global_position.x < global_position.x:
		animated_sprite.flip_h = false
	else:
		animated_sprite.flip_h = true

func move_toward_player() -> void:
	var direction: Vector3 = (player.global_position - global_position).normalized()
	direction.y = 0
	velocity = direction * move_speed
	move_and_slide()

func attack() -> void:
	can_attack = false
	attack_timer.start(attack_cooldown)

	var space_state: PhysicsDirectSpaceState3D = get_world_3d().direct_space_state
	var from: Vector3 = muzzle.global_position
	var to: Vector3 = player.global_position
	var query: PhysicsRayQueryParameters3D = PhysicsRayQueryParameters3D.create(from, to)
	var result: Dictionary = space_state.intersect_ray(query)

	if result and result.collider.is_in_group("player"):
		result.collider.take_damage(attack_damage)

func take_damage(amount: int) -> void:
	if not is_alive:
		return
	health -= amount
	if health <= 0:
		die()

func die() -> void:
	is_alive = false
	set_physics_process(false)
	$CollisionShape3D.set_deferred("disabled", true)
	$DeathTimer.start(3.0)

func _on_death_timer_timeout() -> void:
	queue_free()

func _on_detection_area_body_entered(body: Node3D) -> void:
	if body.is_in_group("player"):
		player = body

func _on_detection_area_body_exited(body: Node3D) -> void:
	if body == player:
		player = null
