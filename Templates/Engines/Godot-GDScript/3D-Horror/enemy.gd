extends CharacterBody3D

@export var patrol_speed: float = 2.0
@export var chase_speed: float = 5.0
@export var detection_range: float = 15.0
@export var chase_range: float = 25.0

@onready var patrol_path: Path3D = $PatrolPath
@onready var path_follow: PathFollow3D = $PatrolPath/PathFollow3D
@onready var detection_area: Area3D = $DetectionArea
@onready var hearing_area: Area3D = $HearingArea

enum State { PATROL, SEARCH, CHASE }
var current_state: State = State.PATROL
var target: Node = null
var last_known_position: Vector3

func _physics_process(delta: float) -> void:
	match current_state:
		State.PATROL:
			patrol(delta)
		State.SEARCH:
			search(delta)
		State.CHASE:
			chase(delta)

func patrol(delta: float) -> void:
	path_follow.progress += patrol_speed * delta
	global_position = path_follow.global_position

func search(delta: float) -> void:
	var direction: Vector3 = (last_known_position - global_position).normalized()
	move_and_collide(direction * patrol_speed * delta)
	if global_position.distance_to(last_known_position) < 1.0:
		current_state = State.PATROL

func chase(delta: float) -> void:
	if target == null:
		current_state = State.SEARCH
		return

	var direction: Vector3 = (target.global_position - global_position).normalized()
	direction.y = 0
	velocity = direction * chase_speed
	move_and_slide()

	if global_position.distance_to(target.global_position) <= 2.0:
		target.die()

	if global_position.distance_to(target.global_position) > chase_range:
		last_known_position = target.global_position
		current_state = State.SEARCH

func _on_detection_area_body_entered(body: Node3D) -> void:
	if body.is_in_group("player"):
		target = body
		current_state = State.CHASE

func _on_detection_area_body_exited(body: Node3D) -> void:
	if body == target:
		last_known_position = target.global_position
		current_state = State.SEARCH

func _on_hearing_area_body_entered(body: Node3D) -> void:
	if body.is_in_group("player") and current_state == State.PATROL:
		last_known_position = body.global_position
		current_state = State.SEARCH
