# Beat-em-up player with combo system, special attacks, knockback, and health
extends CharacterBody2D

@export var speed: float = 250.0
@export var gravity: float = 980.0
@export var attack_damage: int = 10
@export var combo_window: float = 0.5

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var combo_timer: Timer = $ComboTimer
@onready var hitbox: Area2D = $Hitbox
@onready var health_bar: ProgressBar = $HealthBar

var health: int = 100
var max_health: int = 100
var combo_count: int = 0
var is_attacking: bool = false
var can_move: bool = true
var facing_right: bool = true

func _ready() -> void:
	combo_timer.timeout.connect(_on_combo_timeout)
	hitbox.monitoring = false

func _physics_process(delta: float) -> void:
	if not is_on_floor():
		velocity.y += gravity * delta

	if can_move:
		var direction: float = Input.get_axis("move_left", "move_right")
		velocity.x = direction * speed if direction != 0 else 0.0

		if direction != 0:
			facing_right = direction > 0
			animated_sprite.play("walk")
		else:
			animated_sprite.play("idle")
	else:
		velocity.x = 0.0

	animated_sprite.flip_h = not facing_right
	move_and_slide()
	handle_combat()

func handle_combat() -> void:
	if Input.is_action_just_pressed("attack") and not is_attacking:
		start_attack()
	elif Input.is_action_just_pressed("special") and not is_attacking and combo_count >= 3:
		do_special()

func start_attack() -> void:
	is_attacking = true
	can_move = false
	hitbox.monitoring = true
	animated_sprite.play("attack_" + str(min(combo_count, 3)))

func _on_attack_animation_end() -> void:
	is_attacking = false
	can_move = true
	hitbox.monitoring = false
	combo_count += 1
	combo_timer.start(combo_window)
	show_combo_label()

func do_special() -> void:
	is_attacking = true
	can_move = false
	animated_sprite.play("special")
	combo_count = 0

func _on_special_animation_end() -> void:
	is_attacking = false
	can_move = true

func _on_combo_timeout() -> void:
	combo_count = 0

func _on_hitbox_body_entered(body: Node2D) -> void:
	if body.is_in_group("enemy"):
		body.take_damage(attack_damage + (combo_count * 2))
		body.apply_knockback(Vector2(200 if facing_right else -200, -100))

func take_damage(amount: int) -> void:
	health -= amount
	health_bar.value = health
	animated_sprite.play("hit")
	if health <= 0:
		die()

func apply_knockback(force: Vector2) -> void:
	velocity = force

func die() -> void:
	set_physics_process(false)
	animated_sprite.play("death")
	await animated_sprite.animation_finished
	queue_free()

func show_combo_label() -> void:
	if combo_count >= 2:
		var label: Label = $ComboLabel
		label.text = str(combo_count) + "x COMBO!"
		label.show()
		await get_tree().create_timer(0.3).timeout
		label.hide()
