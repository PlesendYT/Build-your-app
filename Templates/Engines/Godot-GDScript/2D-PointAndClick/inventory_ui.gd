extends CanvasLayer

@onready var inventory_grid: GridContainer = $InventoryGrid
@onready var item_detail: Label = $ItemDetail

func _ready() -> void:
	hide()

func _input(event: InputEvent) -> void:
	if event.is_action_pressed("inventory"):
		visible = not visible

func update_inventory(inventory: Array[String]) -> void:
	for child in inventory_grid.get_children():
		child.queue_free()

	for item in inventory:
		var button: Button = Button.new()
		button.text = item
		button.pressed.connect(_on_item_clicked.bind(item))
		inventory_grid.add_child(button)

func _on_item_clicked(item_name: String) -> void:
	item_detail.text = "Selected: " + item_name
