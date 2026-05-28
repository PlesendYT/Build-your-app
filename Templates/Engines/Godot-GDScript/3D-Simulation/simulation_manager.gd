extends Node3D

@export var grid_size: float = 2.0
@export var grid_width: int = 20
@export var grid_depth: int = 20

@onready var camera: Camera3D = $Camera3D
@onready var grid_visual: Node3D = $GridVisual
@onready var building_preview: Node3D = $BuildingPreview

var buildings: Array[Dictionary] = []
var money: float = 1000.0
var income_rate: float = 10.0
var selected_building: String = "house"
var hovered_cell: Vector3 = Vector3.ZERO
var is_placing: bool = false

var building_data: Dictionary = {
	"house": {"cost": 100, "income": 5, "scene": preload("res://house.tscn")},
	"shop": {"cost": 200, "income": 15, "scene": preload("res://shop.tscn")},
	"factory": {"cost": 500, "income": 40, "scene": preload("res://factory.tscn")},
	"park": {"cost": 50, "income": -2, "scene": preload("res://park.tscn")}
}

func _ready() -> void:
	draw_grid()

func _process(delta: float) -> void:
	handle_camera()
	handle_placement()
	update_ui()

func handle_camera() -> void:
	var move: Vector2 = Vector2(
		Input.get_axis("move_left", "move_right"),
		Input.get_axis("move_forward", "move_backward")
	) * 10.0 * delta

	camera.global_position.x += move.x
	camera.global_position.z += move.y

	if Input.is_action_just_pressed("zoom_in"):
		camera.global_position.y = max(5, camera.global_position.y - 2)
	if Input.is_action_just_pressed("zoom_out"):
		camera.global_position.y = min(30, camera.global_position.y + 2)

func handle_placement() -> void:
	var mouse_pos: Vector2 = get_viewport().get_mouse_position()
	var from: Vector3 = camera.project_ray_origin(mouse_pos)
	var to: Vector3 = from + camera.project_ray_normal(mouse_pos) * 100.0
	var space_state: PhysicsDirectSpaceState3D = get_world_3d().direct_space_state
	var query: PhysicsRayQueryParameters3D = PhysicsRayQueryParameters3D.create(from, to)
	var result: Dictionary = space_state.intersect_ray(query)

	if result:
		var hit_pos: Vector3 = result.position
		hovered_cell = snap_to_grid(hit_pos)
		building_preview.global_position = hovered_cell

		if Input.is_action_just_pressed("place") and is_placing:
			place_building()

func snap_to_grid(pos: Vector3) -> Vector3:
	var x: float = round(pos.x / grid_size) * grid_size
	var z: float = round(pos.z / grid_size) * grid_size
	return Vector3(x, 0, z)

func place_building() -> void:
	var data: Dictionary = building_data[selected_building]
	if money < data.cost:
		return

	money -= data.cost
	var building: Node3D = data.scene.instantiate()
	building.global_position = hovered_cell
	add_child(building)
	buildings.append({"type": selected_building, "position": hovered_cell})

func select_building(type: String) -> void:
	selected_building = type
	is_placing = true
	building_preview.visible = true

func _on_timer_timeout() -> void:
	for building in buildings:
		var data: Dictionary = building_data[building.type]
		money += data.income * 0.1

func draw_grid() -> void:
	for x in range(grid_width):
		for z in range(grid_depth):
			var pos: Vector3 = Vector3(x * grid_size - (grid_width * grid_size / 2), 0, z * grid_size - (grid_depth * grid_size / 2))

func update_ui() -> void:
	$UI/MoneyLabel.text = "Money: $" + str(int(money))
	$UI/BuildLabel.text = "Building: " + selected_building
