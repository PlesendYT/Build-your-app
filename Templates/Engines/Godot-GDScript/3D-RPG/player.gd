extends CharacterBody3D

@export var walk_speed: float = 5.0
@export var sprint_speed: float = 8.0
@export var jump_velocity: float = 4.5
@export var gravity: float = 9.8
@export var mouse_sensitivity: float = 0.002

@onready var spring_arm: SpringArm3D = $SpringArm3D
@onready var camera: Camera3D = $SpringArm3D/Camera3D
@onready var animated_sprite: AnimatedSprite3D = $AnimatedSprite3D
@onready var interact_area: Area3D = $InteractArea

var health: int = 100
var max_health: int = 100
var mana: int = 50
var max_mana: int = 50
var level: int = 1
var xp: int = 0
var xp_to_next: int = 100
var inventory: Array[Dictionary] = []
var quests: Array[Dictionary] = []
var is_locked: bool = false

func _ready() -> void:
	Input.set_mouse_mode(Input.MOUSE_MODE_CAPTURED)

func _input(event: InputEvent) -> void:
	if event is InputEventMouseMotion and Input.get_mouse_mode() == Input.MOUSE_MODE_CAPTURED:
		rotate_y(-event.relative.x * mouse_sensitivity)
		spring_arm.rotate_x(-event.relative.y * mouse_sensitivity)
		spring_arm.rotation.x = clamp(spring_arm.rotation.x, -1.0, 0.5)

func _physics_process(delta: float) -> void:
	if is_locked:
		return

	if not is_on_floor():
		velocity.y -= gravity * delta

	var speed: float = sprint_speed if Input.is_action_pressed("sprint") else walk_speed
	var input_dir: Vector2 = Input.get_vector("move_left", "move_right", "move_forward", "move_backward")
	var direction: Vector3 = (transform.basis * Vector3(input_dir.x, 0, -input_dir.y)).normalized()

	if direction:
		velocity.x = direction.x * speed
		velocity.z = direction.z * speed
		animated_sprite.play("run")
		animated_sprite.flip_h = direction.x < 0
	else:
		velocity.x = move_toward(velocity.x, 0, speed)
		velocity.z = move_toward(velocity.z, 0, speed)
		animated_sprite.play("idle")

	if Input.is_action_just_pressed("jump") and is_on_floor():
		velocity.y = jump_velocity

	if Input.is_action_just_pressed("interact"):
		interact()

	move_and_slide()

func interact() -> void:
	var overlapping: Array[Node3D] = interact_area.get_overlapping_areas()
	for area in overlapping:
		if area.is_in_group("interactable"):
			area.interact(self)
			return

func take_damage(amount: int) -> void:
	health -= amount
	if health <= 0:
		die()

func heal(amount: int) -> void:
	health = min(health + amount, max_health)

func add_xp(amount: int) -> void:
	xp += amount
	while xp >= xp_to_next:
		level_up()

func level_up() -> void:
	xp -= xp_to_next
	level += 1
	xp_to_next = floor(xp_to_next * 1.5)
	max_health += 10
	health = max_health
	max_mana += 5
	mana = max_mana

func add_item(item_name: String, quantity: int = 1) -> void:
	for item in inventory:
		if item.name == item_name:
			item.quantity += quantity
			return
	inventory.append({"name": item_name, "quantity": quantity})

func remove_item(item_name: String, quantity: int = 1) -> bool:
	for i in range(inventory.size()):
		if inventory[i].name == item_name:
			inventory[i].quantity -= quantity
			if inventory[i].quantity <= 0:
				inventory.remove_at(i)
			return true
	return false

func add_quest(quest_data: Dictionary) -> void:
	quests.append(quest_data)

func complete_quest(quest_id: String) -> void:
	for quest in quests:
		if quest.id == quest_id and not quest.completed:
			quest.completed = true
			add_xp(quest.xp_reward)
			for reward in quest.item_rewards:
				add_item(reward.name, reward.quantity)
			return

func die() -> void:
	health = max_health
	position = Vector3.ZERO
