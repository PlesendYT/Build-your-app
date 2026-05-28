extends Node2D

@export var room_id: String = "room_1"
@export var room_name: String = "Forgotten Corridor"
@export var connected_rooms: Array[String] = []
@export var enemies: Array[Node] = []

@onready var transition_areas: Node = $TransitionAreas
@onready var camera_limits: Node2D = $CameraLimits

var is_cleared: bool = false
var has_player: bool = false

func _ready() -> void:
	for area in transition_areas.get_children():
		area.room_manager = self
		area.body_entered.connect(_on_player_entered.bind(area))

func _on_player_entered(body: Node2D, area: Area2D) -> void:
	if body.is_in_group("player"):
		var target_room_id: String = area.target_room
		Events.emit_signal("room_changed", room_id, target_room_id, area.spawn_point)
