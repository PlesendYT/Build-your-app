# Tower defense tower with target acquisition, aiming, firing, range visualization, and upgrades
extends Node2D

@export var damage: int = 10
@export var attack_range: float = 150.0
@export var fire_rate: float = 1.0
@export var bullet_scene: PackedScene

@onready var attack_area: Area2D = $AttackArea
@onready var fire_timer: Timer = $FireTimer
@onready var turret_head: Sprite2D = $TurretHead
@onready var range_indicator: Sprite2D = $RangeIndicator

var tower_type: String = "basic"
var current_target: Node2D = null
var upgrade_level: int = 0
var total_cost: int = 0

func _ready() -> void:
	fire_timer.timeout.connect(_fire)
	fire_timer.start(fire_rate)
	attack_area.get_child(0).shape.radius = attack_range
	range_indicator.scale = Vector2(attack_range / 64, attack_range / 64)

func _physics_process(delta: float) -> void:
	find_target()
	if current_target != null:
		aim_at_target()

func find_target() -> void:
	var enemies: Array[Node] = get_tree().get_nodes_in_group("enemies")
	current_target = null
	var closest_distance: float = INF

	for enemy in enemies:
		var dist: float = global_position.distance_to(enemy.global_position)
		if dist <= attack_range and dist < closest_distance:
			closest_distance = dist
			current_target = enemy

func aim_at_target() -> void:
	if current_target == null:
		return
	turret_head.look_at(current_target.global_position)

func _fire() -> void:
	if current_target == null or not is_instance_valid(current_target):
		return

	var bullet: Area2D = bullet_scene.instantiate()
	bullet.global_position = turret_head.global_position
	bullet.direction = (current_target.global_position - global_position).normalized()
	bullet.damage = damage
	get_parent().add_child(bullet)

func upgrade() -> void:
	var upgrade_cost: int = get_upgrade_cost()
	if upgrade_level < 3:
		damage += 5
		attack_range += 20
		fire_rate = max(0.1, fire_rate - 0.1)
		upgrade_level += 1

func get_upgrade_cost() -> int:
	return 50 + (upgrade_level * 25)

func _on_attack_area_mouse_entered() -> void:
	range_indicator.show()

func _on_attack_area_mouse_exited() -> void:
	range_indicator.hide()
