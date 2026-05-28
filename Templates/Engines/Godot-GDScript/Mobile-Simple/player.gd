extends CharacterBody2D

@export var move_speed: float = 300.0
@export var tap_tolerance: float = 30.0

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var target_marker: Sprite2D = $TargetMarker

var touch_start: Vector2 = Vector2.ZERO
var is_moving_to_target: bool = false
var move_target: Vector2 = Vector2.ZERO
var swipe_threshold: float = 50.0

func _ready() -> void:
	target_marker.hide()

func _input(event: InputEvent) -> void:
	if event is InputEventScreenTouch:
		if event.pressed:
			touch_start = event.position
		else:
			var swipe_distance: float = touch_start.distance_to(event.position)
			if swipe_distance < tap_tolerance:
				handle_tap(event.position)
			else:
				handle_swipe(touch_start, event.position)

func handle_tap(screen_pos: Vector2) -> void:
	move_target = get_global_mouse_position()
	target_marker.global_position = move_target
	target_marker.show()
	is_moving_to_target = true

	var tap_tween: Tween = create_tween()
	tap_tween.tween_property(target_marker, "modulate:a", 0.0, 0.5)
	await tap_tween.finished
	target_marker.modulate.a = 1.0
	target_marker.hide()

func handle_swipe(start: Vector2, end: Vector2) -> void:
	var direction: Vector2 = (end - start).normalized()

	if abs(direction.x) > abs(direction.y):
		if direction.x > 0:
			dash(Vector2.RIGHT)
		else:
			dash(Vector2.LEFT)
	else:
		if direction.y > 0:
			dash(Vector2.DOWN)
		else:
			dash(Vector2.UP)

func dash(direction: Vector2) -> void:
	velocity = direction * move_speed * 2
	move_and_slide()
	await get_tree().create_timer(0.2).timeout
	velocity = Vector2.ZERO

func _physics_process(delta: float) -> void:
	if is_moving_to_target:
		var direction: Vector2 = (move_target - global_position).normalized()
		velocity = direction * move_speed

		if global_position.distance_to(move_target) < 5.0:
			is_moving_to_target = false
			velocity = Vector2.ZERO

		move_and_slide()
		update_animation(direction)
	else:
		animated_sprite.play("idle")

func update_animation(direction: Vector2) -> void:
	animated_sprite.play("walk")
	animated_sprite.flip_h = direction.x < 0
