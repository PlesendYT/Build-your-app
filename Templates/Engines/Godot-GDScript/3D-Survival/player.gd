extends CharacterBody3D

@export var walk_speed: float = 4.0
@export var run_speed: float = 7.0
@export var jump_velocity: float = 5.0
@export var gravity: float = 15.0

@onready var camera_pivot: Node3D = $CameraPivot
@onready var camera: Camera3D = $CameraPivot/Camera3D
@onready var inventory_ui: Control = $UI/InventoryUI
@onready var crosshair: TextureRect = $UI/Crosshair

var health: float = 100.0
var max_health: float = 100.0
var hunger: float = 100.0
var thirst: float = 100.0
var temperature: float = 37.0
var inventory: Array[Dictionary] = []
var hotbar: Array[Dictionary] = []
var selected_hotbar: int = 0
var is_building: bool = false

func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func _input(event: InputEvent) -> void:
	if event is InputEventMouseMotion and Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
		rotate_y(-event.relative.x * 0.003)
		camera_pivot.rotate_x(-event.relative.y * 0.003)
		camera_pivot.rotation.x = clamp(camera_pivot.rotation.x, -1.4, 1.4)

	if event.is_action_pressed("inventory"):
		inventory_ui.visible = not inventory_ui.visible
		Input.set_mouse_mode(Input.MOUSE_MODE_VISIBLE if inventory_ui.visible else Input.MOUSE_MODE_CAPTURED)

	for i in range(10):
		if event.is_action_pressed("hotbar_" + str(i)):
			selected_hotbar = i

func _physics_process(delta: float) -> void:
	if inventory_ui.visible:
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
	update_vitals(delta)
	handle_interaction()

func update_vitals(delta: float) -> void:
	hunger -= 0.5 * delta
	thirst -= 0.8 * delta

	if hunger <= 0 or thirst <= 0:
		health -= 2.0 * delta

	if hunger > 50:
		health = min(health + 0.5 * delta, max_health)

	if temperature < 35 or temperature > 40:
		health -= 1.0 * delta

func handle_interaction() -> void:
	if Input.is_action_just_pressed("interact"):
		var space_state: PhysicsDirectSpaceState3D = get_world_3d().direct_space_state
		var from: Vector3 = camera.global_position
		var to: Vector3 = from - camera.global_transform.basis.z * 3.0
		var query: PhysicsRayQueryParameters3D = PhysicsRayQueryParameters3D.create(from, to)
		var result: Dictionary = space_state.intersect_ray(query)

		if result:
			var target: Node = result.collider
			if target.is_in_group("resource"):
				gather_resource(target)
			elif target.is_in_group("pickup"):
				pickup_item(target)

func gather_resource(resource: Node) -> void:
	var item_name: String = resource.resource_type
	add_to_inventory(item_name, 1)
	resource.queue_free()

func pickup_item(item: Node) -> void:
	var item_data: Dictionary = item.item_data
	add_to_inventory(item_data.name, item_data.quantity)
	item.queue_free()

func add_to_inventory(item_name: String, quantity: int) -> void:
	for slot in inventory:
		if slot.name == item_name:
			slot.quantity += quantity
			return
	inventory.append({"name": item_name, "quantity": quantity})

func craft_item(recipe: Dictionary) -> bool:
	for ingredient in recipe.ingredients:
		if not has_item(ingredient.name, ingredient.quantity):
			return false

	for ingredient in recipe.ingredients:
		remove_from_inventory(ingredient.name, ingredient.quantity)

	add_to_inventory(recipe.result, recipe.quantity)
	return true

func has_item(item_name: String, quantity: int) -> bool:
	for slot in inventory:
		if slot.name == item_name and slot.quantity >= quantity:
			return true
	return false

func remove_from_inventory(item_name: String, quantity: int) -> void:
	for i in range(inventory.size()):
		if inventory[i].name == item_name:
			inventory[i].quantity -= quantity
			if inventory[i].quantity <= 0:
				inventory.remove_at(i)
			return

func take_damage(amount: float) -> void:
	health -= amount
	if health <= 0:
		die()

func die() -> void:
	get_tree().reload_current_scene()
