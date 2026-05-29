# Racing game vehicle controller with physics-based acceleration, steering, and lap tracking
extends RigidBody3D

@export var engine_power: float = 800.0
@export var brake_power: float = 300.0
@export var steering_angle: float = 0.5
@export var max_speed: float = 30.0

@onready var wheel_fl: Node3D = $WheelFL
@onready var wheel_fr: Node3D = $WheelFR
@onready var wheel_rl: Node3D = $WheelRL
@onready var wheel_rr: Node3D = $WheelRR
@onready var speed_label: Label3D = $SpeedLabel

var current_speed: float = 0.0
var current_lap: int = 0
var total_laps: int = 3
var lap_started: bool = false
var checkpoints_passed: int = 0
var total_checkpoints: int = 0

func _ready() -> void:
	total_checkpoints = get_tree().get_nodes_in_group("checkpoint").size()

func _physics_process(delta: float) -> void:
	var throttle: float = 0.0
	var brake: float = 0.0
	var steer: float = 0.0

	if Input.is_action_pressed("accelerate"):
		throttle = 1.0
	if Input.is_action_pressed("brake"):
		brake = 1.0

	steer = Input.get_axis("steer_right", "steer_left")

	var acceleration: Vector3 = -global_transform.basis.z * throttle * engine_power
	if current_speed < max_speed:
		apply_central_force(acceleration)

	if brake > 0:
		apply_central_force(global_transform.basis.z * brake_power)

	if abs(linear_velocity.length()) > 0.5:
		apply_torque(Vector3(0, steer * steering_angle * linear_velocity.length(), 0))

	current_speed = linear_velocity.length()
	speed_label.text = str(int(current_speed * 3.6)) + " km/h"

func _on_checkpoint_passed(checkpoint_id: int) -> void:
	if checkpoint_id == checkpoints_passed:
		checkpoints_passed += 1

		if checkpoints_passed >= total_checkpoints:
			complete_lap()

func complete_lap() -> void:
	current_lap += 1
	checkpoints_passed = 0

	if current_lap >= total_laps:
		race_finished()

func race_finished() -> void:
	set_physics_process(false)
	Events.emit_signal("race_finished", current_lap)
