# FPS player controller with gun mechanics, ammo/reload, armor, and health
extends CharacterBody3D

@export var mouse_sensitivity: float = 0.002
@export var walk_speed: float = 5.0
@export var sprint_speed: float = 8.0
@export var jump_velocity: float = 4.5
@export var gravity: float = 9.8

@onready var head: Node3D = $Head
@onready var camera: Camera3D = $Head/Camera3D
@onready var gun_aim: Node3D = $Head/GunAim
@onready var gun_timer: Timer = $GunTimer
@onready var reload_timer: Timer = $ReloadTimer

var health: int = 100
var max_health: int = 100
var armor: int = 0
var ammo: int = 30
var max_ammo: int = 30
var reserve_ammo: int = 90
var is_reloading: bool = false
var can_shoot: bool = true

func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func _input(event: InputEvent) -> void:
	if event is InputEventMouseMotion and Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
		head.rotate_y(-event.relative.x * mouse_sensitivity)
		camera.rotate_x(-event.relative.y * mouse_sensitivity)
		camera.rotation.x = clamp(camera.rotation.x, -1.4, 1.4)

func _physics_process(delta: float) -> void:
	if not is_on_floor():
		velocity.y -= gravity * delta

	var speed: float = sprint_speed if Input.is_action_pressed("sprint") else walk_speed
	var input_dir: Vector2 = Input.get_vector("move_left", "move_right", "move_forward", "move_backward")
	var direction: Vector3 = (head.transform.basis * Vector3(input_dir.x, 0, -input_dir.y)).normalized()

	if direction:
		velocity.x = direction.x * speed
		velocity.z = direction.z * speed
	else:
		velocity.x = move_toward(velocity.x, 0, speed)
		velocity.z = move_toward(velocity.z, 0, speed)

	if Input.is_action_just_pressed("jump") and is_on_floor():
		velocity.y = jump_velocity

	move_and_slide()
	handle_shooting()

func handle_shooting() -> void:
	if Input.is_action_pressed("shoot") and can_shoot and not is_reloading and ammo > 0:
		shoot()
	elif Input.is_action_just_pressed("reload") and ammo < max_ammo and reserve_ammo > 0:
		start_reload()

func shoot() -> void:
	can_shoot = false
	ammo -= 1
	gun_timer.start(0.1)

	var space_state: PhysicsDirectSpaceState3D = get_world_3d().direct_space_state
	var crosshair_pos: Vector2 = get_viewport().size / 2
	var from: Vector3 = camera.global_position
	var to: Vector3 = camera.project_position(crosshair_pos, 1000.0)
	var query: PhysicsRayQueryParameters3D = PhysicsRayQueryParameters3D.create(from, to)
	var result: Dictionary = space_state.intersect_ray(query)

	if result:
		var target: Node = result.collider
		if target.is_in_group("enemy"):
			target.take_damage(25)
			create_hit_effect(result.position)

func start_reload() -> void:
	is_reloading = true
	reload_timer.start(2.0)

func _on_reload_timer_timeout() -> void:
	var needed: int = max_ammo - ammo
	var available: int = min(needed, reserve_ammo)
	ammo += available
	reserve_ammo -= available
	is_reloading = false

func _on_gun_timer_timeout() -> void:
	can_shoot = true

func take_damage(amount: int) -> void:
	if armor > 0:
		var armor_absorb: int = min(armor, amount * 0.5)
		armor -= armor_absorb
		amount -= armor_absorb

	health -= amount
	if health <= 0:
		die()

func die() -> void:
	get_tree().reload_current_scene()

func add_health(amount: int) -> void:
	health = min(health + amount, max_health)

func add_ammo(amount: int) -> void:
	reserve_ammo += amount

func create_hit_effect(position: Vector3) -> void:
	var effect: Node3D = preload("res://hit_effect.tscn").instantiate()
	effect.global_position = position
	get_parent().add_child(effect)
