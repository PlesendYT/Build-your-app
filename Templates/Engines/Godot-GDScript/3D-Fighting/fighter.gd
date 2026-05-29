# 3D fighting game fighter controller
# Supports two-player movement, combo attack system, hit detection, and damage
extends CharacterBody3D

@export var speed: float = 8.0
@export var gravity: float = 15.0
@export var health: int = 100
@export var max_health: int = 100
@export var player_id: int = 1

@onready var anim_tree: AnimationTree = $AnimationTree
@onready var anim_state: AnimationNodeStateMachinePlayback = anim_tree.get("parameters/playback")
@onready var hitbox: Area3D = $Hitbox
@onready var health_bar: ProgressBar = $UI/HealthBar

var combo_input: Array[String] = []
var last_input_time: float = 0.0
var input_buffer_time: float = 0.2
var is_attacking: bool = false
var combo_count: int = 0
var facing_direction: float = 1.0

func _ready() -> void:
	health_bar.max_value = max_health
	health_bar.value = health

func _physics_process(delta: float) -> void:
	if not is_on_floor():
		velocity.y -= gravity * delta

	var move_input: float = 0.0
	if player_id == 1:
		move_input = Input.get_axis("p1_left", "p1_right")
	else:
		move_input = Input.get_axis("p2_left", "p2_right")

	velocity.x = move_input * speed

	if move_input != 0:
		facing_direction = move_input

	move_and_slide()
	handle_input()
	update_facing()

func handle_input() -> void:
	if player_id == 1:
		if Input.is_action_just_pressed("p1_punch"):
			try_attack("punch")
		elif Input.is_action_just_pressed("p1_kick"):
			try_attack("kick")
		elif Input.is_action_just_pressed("p1_special"):
			try_attack("special")
	else:
		if Input.is_action_just_pressed("p2_punch"):
			try_attack("punch")
		elif Input.is_action_just_pressed("p2_kick"):
			try_attack("kick")
		elif Input.is_action_just_pressed("p2_special"):
			try_attack("special")

func try_attack(attack_type: String) -> void:
	if is_attacking:
		combo_input.append(attack_type)
		return

	is_attacking = true
	combo_count = min(combo_count + 1, 3)
	hitbox.monitoring = true

	var anim_name: String = attack_type + "_" + str(combo_count)
	anim_state.travel(anim_name)

func _on_attack_end() -> void:
	is_attacking = false
	hitbox.monitoring = false

	if combo_input.size() > 0:
		var next_attack: String = combo_input.pop_front()
		try_attack(next_attack)

func _on_hitbox_body_entered(body: Node3D) -> void:
	if body.is_in_group("fighter") and body != self:
		var damage: int = 10 + (combo_count * 5)
		body.take_damage(damage)

func take_damage(amount: int) -> void:
	health -= amount
	health_bar.value = health
	anim_state.travel("hit")
	combo_count = 0

	if health <= 0:
		die()

func die() -> void:
	anim_state.travel("knockdown")
	set_physics_process(false)
	Events.emit_signal("round_end", player_id)

func update_facing() -> void:
	scale.x = facing_direction
