# Puzzle drop zone that accepts a specific piece type
extends Area2D

@export var piece_type: String = "blue"

var is_occupied: bool = false
