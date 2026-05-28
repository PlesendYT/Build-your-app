extends CharacterBody3D

@export var walk_speed: float = 3.0
@export var crouch_speed: float = 1.5
@export var mouse_sensitivity: float = 0.002
@export var flashlight_drain: float = 0.5
@export var sanity_drain_dark: float = 1.0

@onready var head: Node3D = $Head
@onready var camera: Camera3D = $Head/Camera3D
@onready var flashlight: SpotLight3D = $Head/Flashlight
@onready var battery_label: Label = $UI/BatteryLabel
@onready var sanity_bar: ProgressBar = $UI/SanityBar

var battery: float = 100.0
var sanity: float = 100.0
var is_hiding: bool = false
var is_crouching: bool = false
var is_dead: bool = false

func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func _input(event: InputEvent) -> void:
	if event is InputEventMouseMotion and Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
		head.rotate_y(-event.relative.x * mouse_sensitivity)
		camera.rotate_x(-event.relative.y * mouse_sensitivity)
		camera.rotation.x = clamp(camera.rotation.x, -1.4, 1.4)

	if event.is_action_pressed("toggle_flashlight"):
		flashlight.visible = not flashlight.visible

func _physics_process(delta: float) -> void:
	if is_dead or is_hiding:
		return

	handle_movement(delta)
	handle_flashlight(delta)
	handle_sanity(delta)
	update_ui()

func handle_movement(delta: float) -> void:
	var speed: float = crouch_speed if Input.is_action_pressed("crouch") else walk_speed
	var input_dir: Vector2 = Input.get_vector("move_left", "move_right", "move_forward", "move_backward")
	var direction: Vector3 = (head.transform.basis * Vector3(input_dir.x, 0, -input_dir.y)).normalized()

	velocity.x = direction.x * speed
	velocity.z = direction.z * speed

	if is_on_floor() and Input.is_action_just_pressed("jump"):
		velocity.y = 3.0

	if not is_on_floor():
		velocity.y -= 9.8 * delta

	move_and_slide()

func handle_flashlight(delta: float) -> void:
	if flashlight.visible:
		battery -= flashlight_drain * delta
		if battery <= 0:
			battery = 0
			flashlight.visible = false

func handle_sanity(delta: float) -> void:
	if not flashlight.visible and not is_hiding:
		sanity -= sanity_drain_dark * delta
	elif is_hiding:
		sanity += 2.0 * delta

	sanity = clamp(sanity, 0.0, 100.0)

	if sanity <= 0:
		die()

func enter_hiding_spot() -> void:
	is_hiding = true
	visible = false
	Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE)

func leave_hiding_spot() -> void:
	is_hiding = false
	visible = true
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func die() -> void:
	is_dead = true
	get_tree().reload_current_scene()

func update_ui() -> void:
	battery_label.text = "Battery: " + str(int(battery)) + "%"
	sanity_bar.value = sanity
