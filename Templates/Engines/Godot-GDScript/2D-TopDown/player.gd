extends CharacterBody2D

@export var speed: float = 200.0
@export var dash_speed: float = 600.0
@export var dash_duration: float = 0.2
@export var dash_cooldown: float = 1.0

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var dash_timer: Timer = $DashTimer
@onready var cooldown_timer: Timer = $CooldownTimer
@onready var interact_ray: RayCast2D = $InteractRay

var is_dashing: bool = false
var can_dash: bool = true
var facing_direction: Vector2 = Vector2.DOWN
var inventory: Array[String] = []

func _ready() -> void:
	dash_timer.timeout.connect(_on_dash_end)

func _physics_process(delta: float) -> void:
	if is_dashing:
		move_and_slide()
		return

	var direction: Vector2 = Vector2(
		Input.get_axis("move_left", "move_right"),
		Input.get_axis("move_up", "move_down")
	).normalized()

	if direction != Vector2.ZERO:
		velocity = direction * speed
		facing_direction = direction
		update_animation(direction)
	else:
		velocity = Vector2.ZERO
		animated_sprite.play("idle")

	move_and_slide()
	handle_dash()
	handle_interaction()

func update_animation(direction: Vector2) -> void:
	animated_sprite.play("walk")
	if abs(direction.x) > abs(direction.y):
		animated_sprite.flip_h = direction.x < 0
		animated_sprite.animation = "walk_side"
	elif direction.y < 0:
		animated_sprite.animation = "walk_up"
	else:
		animated_sprite.animation = "walk_down"

func handle_dash() -> void:
	if Input.is_action_just_pressed("dash") and can_dash and not is_dashing:
		is_dashing = true
		can_dash = false
		velocity = facing_direction * dash_speed
		dash_timer.start(dash_duration)
		cooldown_timer.start(dash_cooldown)

func _on_dash_end() -> void:
	is_dashing = false
	velocity = Vector2.ZERO

func handle_interaction() -> void:
	if Input.is_action_just_pressed("interact"):
		if interact_ray.is_colliding():
			var target: Node = interact_ray.get_collider()
			if target.has_method("interact"):
				target.interact(self)

func add_item(item_name: String) -> void:
	inventory.append(item_name)
