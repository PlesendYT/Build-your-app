extends CharacterBody3D

@export var walk_speed: float = 6.0
@export var run_speed: float = 10.0
@export var jump_velocity: float = 6.0
@export var gravity: float = 15.0
@export var mouse_sensitivity: float = 0.003

@onready var spring_arm: SpringArm3D = $SpringArm3D
@onready var camera: Camera3D = $SpringArm3D/Camera3D
@onready var anim_player: AnimationPlayer = $AnimationPlayer
@onready var minimap: Control = $UI/Minimap
@onready var waypoint_icon: TextureRect = $UI/WaypointIcon

var current_vehicle: Node = null
var in_vehicle: bool = false
var active_quests: Array[Dictionary] = []
var discovered_locations: Array[String] = []
var waypoints: Array[Vector3] = []

func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func _input(event: InputEvent) -> void:
	if in_vehicle:
		return

	if event is InputEventMouseMotion and Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
		rotate_y(-event.relative.x * mouse_sensitivity)
		spring_arm.rotate_x(-event.relative.y * mouse_sensitivity)
		spring_arm.rotation.x = clamp(spring_arm.rotation.x, -1.0, 0.5)

	if event.is_action_pressed("interact"):
		try_interact()

func _physics_process(delta: float) -> void:
	if in_vehicle:
		return

	velocity.y -= gravity * delta
	var speed: float = run_speed if Input.is_action_pressed("sprint") else walk_speed
	var input_dir: Vector2 = Input.get_vector("move_left", "move_right", "move_forward", "move_backward")
	var direction: Vector3 = (transform.basis * Vector3(input_dir.x, 0, -input_dir.y)).normalized()

	velocity.x = direction.x * speed
	velocity.z = direction.z * speed

	if Input.is_action_just_pressed("jump") and is_on_floor():
		velocity.y = jump_velocity

	move_and_slide()

	if direction.length() > 0.1:
		anim_player.play("run")
	else:
		anim_player.play("idle")

	update_minimap()

func try_interact() -> void:
	var space_state: PhysicsDirectSpaceState3D = get_world_3d().direct_space_state
	var from: Vector3 = camera.global_position
	var to: Vector3 = from - camera.global_transform.basis.z * 5.0
	var query: PhysicsRayQueryParameters3D = PhysicsRayQueryParameters3D.create(from, to)
	var result: Dictionary = space_state.intersect_ray(query)

	if result:
		var target: Node = result.collider
		if target.is_in_group("vehicle"):
			enter_vehicle(target)
		elif target.is_in_group("quest_giver"):
			target.start_quest(self)
		elif target.is_in_group("location"):
			discover_location(target.location_name)

func enter_vehicle(vehicle: Node) -> void:
	current_vehicle = vehicle
	in_vehicle = true
	visible = false
	vehicle.enter(self)
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func exit_vehicle() -> void:
	if current_vehicle:
		global_position = current_vehicle.global_position + Vector3(3, 0, 0)
		current_vehicle.exit()
		current_vehicle = null
		in_vehicle = false
		visible = true

func discover_location(location_name: String) -> void:
	if not location_name in discovered_locations:
		discovered_locations.append(location_name)
		Events.emit_signal("location_discovered", location_name)

func add_waypoint(position: Vector3) -> void:
	waypoints.append(position)

func update_minimap() -> void:
	pass

func take_damage(amount: int) -> void:
	health -= amount
	if health <= 0:
		die()

var health: int = 100
var max_health: int = 100

func die() -> void:
	global_position = Vector3.ZERO
	health = max_health
