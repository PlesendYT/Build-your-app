# 3D RPG NPC with dialogue and optional merchant shop functionality
extends Area3D

@export var npc_name: String = "Merchant"
@export var dialogue: Array[Dictionary] = []
@export var is_merchant: bool = false
@export var shop_items: Array[Dictionary] = []

@onready var label: Label3D = $Label3D

func _ready() -> void:
	label.text = npc_name
	add_to_group("interactable")

func interact(player: Node) -> void:
	if dialogue.size() > 0:
		var dialogue_manager: Node = get_tree().get_first_node_in_group("dialogue_manager")
		if dialogue_manager:
			dialogue_manager.start_dialogue(dialogue, self, player)

	if is_merchant and shop_items.size() > 0:
		var shop_ui: Node = get_tree().get_first_node_in_group("shop_ui")
		if shop_ui:
			shop_ui.open_shop(shop_items, player)
