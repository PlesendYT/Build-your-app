# Point-and-click hotspot with item requirements, locked states, and interaction feedback
extends Area2D

@export var hotspot_name: String = "Door"
@export var interaction_text: String = "Open the door?"
@export var required_item: String = ""
@export var gives_item: String = ""
@export var is_locked: bool = false

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var label: Label = $Label

func _ready() -> void:
	label.text = hotspot_name
	input_event.connect(_on_input_event)

func _on_input_event(viewport: Node, event: InputEvent, shape_idx: int) -> void:
	if event is InputEventMouseButton and event.pressed and event.button_index == MOUSE_BUTTON_LEFT:
		var player: Node = get_tree().get_first_node_in_group("player")
		if player != null:
			player.interact_with_hotspot(self)

func on_interact() -> void:
	if is_locked:
		show_message("It's locked.")
		return

	if required_item != "" and not has_item(required_item):
		show_message("I need " + required_item + " for this.")
		return

	show_message(interaction_text)
	animated_sprite.play("activate")

	if gives_item != "":
		var player: Node = get_tree().get_first_node_in_group("player")
		player.add_item(gives_item)

func show_message(text: String) -> void:
	var message_label: Label = $MessageLabel
	message_label.text = text
	message_label.show()
	await get_tree().create_timer(2.0).timeout
	message_label.hide()

func has_item(item_name: String) -> bool:
	var player: Node = get_tree().get_first_node_in_group("player")
	if player == null:
		return false
	return item_name in player.inventory
