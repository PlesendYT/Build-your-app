# Card game UI: displays hand cards, mana, health, and end-turn button
extends Control

@onready var hand_container: HBoxContainer = $HandContainer
@onready var mana_label: Label = $ManaLabel
@onready var health_label: Label = $HealthLabel
@onready var end_turn_button: Button = $EndTurnButton

var card_ui_scene: PackedScene = preload("res://card_ui.tscn")
var game_manager: Node

func _ready() -> void:
	game_manager = get_parent().get_node("CardGameManager")
	game_manager.hand_updated.connect(_on_hand_updated)
	game_manager.mana_updated.connect(_on_mana_updated)
	game_manager.health_updated.connect(_on_health_updated)
	end_turn_button.pressed.connect(_on_end_turn)

func _on_hand_updated(hand: Array) -> void:
	for child in hand_container.get_children():
		child.queue_free()

	for i in hand.size():
		var card_ui: Control = card_ui_scene.instantiate()
		card_ui.setup(hand[i], i)
		card_ui.card_played.connect(_on_card_played)
		hand_container.add_child(card_ui)

func _on_card_played(card_index: int) -> void:
	game_manager.play_card(card_index, get_random_enemy())

func _on_mana_updated(current: int, max_mana: int) -> void:
	mana_label.text = str(current) + " / " + str(max_mana)

func _on_health_updated(current: int, max_hp: int) -> void:
	health_label.text = str(current) + " / " + str(max_hp)

func _on_end_turn() -> void:
	game_manager.end_turn()

func get_random_enemy() -> Node:
	var enemies: Array[Node] = get_tree().get_nodes_in_group("enemies")
	if enemies.size() > 0:
		return enemies[randi() % enemies.size()]
	return null
