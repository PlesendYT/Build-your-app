# Drag-and-drop puzzle piece with type matching and position tracking
extends Area2D

@export var piece_type: String = "blue"
@export var start_position: Vector2

var is_placed: bool = false
var is_dragging: bool = false

signal piece_dragged(piece, global_mouse_pos)
signal piece_dropped(piece)

func _ready() -> void:
	start_position = position

func _input_event(viewport: Node, event: InputEvent, shape_idx: int) -> void:
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT:
			if event.pressed and not is_placed:
				is_dragging = true
				piece_dragged.emit(self, get_global_mouse_position())
			elif not event.pressed and is_dragging:
				is_dragging = false
				piece_dropped.emit(self)
