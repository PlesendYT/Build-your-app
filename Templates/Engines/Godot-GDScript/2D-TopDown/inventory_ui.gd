# Top-down inventory UI with toggle and grid display
extends CanvasLayer

@onready var inventory_grid: GridContainer = $InventoryGrid
@onready var item_label: Label = $ItemLabel

var is_open: bool = false

func _ready() -> void:
	hide()

func _input(event: InputEvent) -> void:
	if event.is_action_pressed("inventory"):
		is_open = not is_open
		visible = is_open
		get_tree().paused = is_open

func update_inventory_display(inventory: Array[String]) -> void:
	for child in inventory_grid.get_children():
		child.queue_free()

	for item in inventory:
		var item_icon: TextureRect = TextureRect.new()
		item_label.text = item
		inventory_grid.add_child(item_icon)
