extends CharacterBody2D

@export var walk_speed: float = 200.0

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var navigation_agent: NavigationAgent2D = $NavigationAgent2D

var target_position: Vector2
var is_moving: bool = false

func _ready() -> void:
	navigation_agent.path_desired_distance = 4.0
	navigation_agent.target_desired_distance = 4.0

func _input(event: InputEvent) -> void:
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_RIGHT:
		target_position = get_global_mouse_position()
		navigation_agent.target_position = target_position
		is_moving = true

func _physics_process(delta: float) -> void:
	if not is_moving:
		return

	if navigation_agent.is_navigation_finished():
		is_moving = false
		velocity = Vector2.ZERO
		animated_sprite.play("idle")
		return

	var next_position: Vector2 = navigation_agent.get_next_path_position()
	var direction: Vector2 = (next_position - global_position).normalized()
	velocity = direction * walk_speed
	animated_sprite.play("walk")
	animated_sprite.flip_h = direction.x < 0

	move_and_slide()

func interact_with_hotspot(hotspot: Node) -> void:
	if hotspot.has_method("on_interact"):
		hotspot.on_interact()
