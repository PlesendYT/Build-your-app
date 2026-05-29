# Puzzle manager: handles drag-and-drop, snapping to zones, piece return, and completion
extends Node2D

@export var snap_distance: float = 30.0
@export var grid_size: Vector2 = Vector2(64, 64)

@onready var puzzle_pieces: Node = $PuzzlePieces
@onready var drop_zones: Node = $DropZones
@onready var completion_sound: AudioStreamPlayer2D = $CompletionSound

var placed_pieces: int = 0
var total_pieces: int = 0
var dragging_piece: Area2D = null
var drag_offset: Vector2 = Vector2.ZERO

func _ready() -> void:
	total_pieces = puzzle_pieces.get_child_count()
	for piece in puzzle_pieces.get_children():
		piece.connect("piece_dragged", _on_piece_dragged)
		piece.connect("piece_dropped", _on_piece_dropped)

func _on_piece_dragged(piece: Area2D, global_mouse_pos: Vector2) -> void:
	dragging_piece = piece
	drag_offset = piece.global_position - global_mouse_pos
	piece.z_index = 10

func _on_piece_dropped(piece: Area2D) -> void:
	piece.z_index = 1
	var closest_zone: Area2D = find_closest_zone(piece.global_position)

	if closest_zone != null and closest_zone.piece_type == piece.piece_type:
		snap_piece(piece, closest_zone)
	else:
		return_piece(piece)

	dragging_piece = null

func _process(delta: float) -> void:
	if dragging_piece != null:
		dragging_piece.global_position = get_global_mouse_position() + drag_offset

func find_closest_zone(global_pos: Vector2) -> Area2D:
	for zone in drop_zones.get_children():
		if global_pos.distance_to(zone.global_position) < snap_distance:
			if zone.is_occupied == false:
				return zone
	return null

func snap_piece(piece: Area2D, zone: Area2D) -> void:
	piece.global_position = zone.global_position
	piece.is_placed = true
	zone.is_occupied = true
	piece.set_process(false)
	placed_pieces += 1

	if placed_pieces >= total_pieces:
		on_puzzle_complete()

func return_piece(piece: Area2D) -> void:
	var tween: Tween = create_tween()
	tween.tween_property(piece, "global_position", piece.start_position, 0.3)
	piece.set_process(false)

	await tween.finished
	piece.set_process(true)

func on_puzzle_complete() -> void:
	completion_sound.play()
	await get_tree().create_timer(1.0).timeout
	get_tree().call_group("level_manager", "on_level_complete")
