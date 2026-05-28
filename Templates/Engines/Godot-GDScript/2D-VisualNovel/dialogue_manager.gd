extends CanvasLayer

@export var text_speed: float = 0.03
@export var auto_advance: bool = false

@onready var name_label: Label = $DialoguePanel/NameLabel
@onready var text_label: RichTextLabel = $DialoguePanel/TextLabel
@onready var character_sprite: TextureRect = $CharacterSprite
@onready var background_sprite: TextureRect = $BackgroundSprite
@onready var choices_container: VBoxContainer = $ChoicesContainer
@onready var next_indicator: TextureRect = $NextIndicator
@onready var advance_timer: Timer = $AdvanceTimer

var dialogue_data: Array[Dictionary] = []
var current_line: int = 0
var is_displaying: bool = false
var is_typing: bool = false
var current_text: String = ""
var displayed_text: String = ""
var choice_mode: bool = false
var variables: Dictionary = {}

signal dialogue_ended

func _ready() -> void:
	advance_timer.timeout.connect(_auto_advance)
	hide()

func load_dialogue(data: Array[Dictionary]) -> void:
	dialogue_data = data
	current_line = 0
	show()
	display_line()

func display_line() -> void:
	if current_line >= dialogue_data.size():
		end_dialogue()
		return

	var line: Dictionary = dialogue_data[current_line]
	choice_mode = false

	if line.has("choice"):
		show_choices(line.choices)
		return

	if line.has("background"):
		change_background(line.background)

	if line.has("character"):
		show_character(line.character, line.get("expression", "normal"))

	if line.has("name"):
		name_label.text = line.name
	else:
		name_label.text = ""

	if line.has("text"):
		current_text = line.text
		start_typing()

	if line.has("set_variable"):
		variables[line.set_variable.key] = line.set_variable.value

func start_typing() -> void:
	is_typing = true
	is_displaying = true
	displayed_text = ""
	text_label.text = ""
	next_indicator.hide()

	while displayed_text.length() < current_text.length():
		displayed_text += current_text[displayed_text.length()]
		text_label.text = displayed_text
		await get_tree().create_timer(text_speed).timeout

	is_typing = false
	next_indicator.show()

	if auto_advance:
		advance_timer.start(2.0)

func _input(event: InputEvent) -> void:
	if not is_displaying:
		return

	if event.is_action_pressed("ui_accept") or event.is_action_pressed("ui_click"):
		if is_typing:
			complete_text()
		elif not choice_mode:
			advance_line()

func complete_text() -> void:
	displayed_text = current_text
	text_label.text = current_text
	is_typing = false
	next_indicator.show()

func advance_line() -> void:
	current_line += 1
	display_line()

func show_choices(choices: Array[Dictionary]) -> void:
	choice_mode = true
	for child in choices_container.get_children():
		child.queue_free()

	for choice in choices:
		var button: Button = Button.new()
		button.text = choice.text
		button.pressed.connect(_on_choice_selected.bind(choice.target_line))
		choices_container.add_child(button)

	choices_container.show()
	next_indicator.hide()

func _on_choice_selected(target_line: int) -> void:
	choices_container.hide()
	for child in choices_container.get_children():
		child.queue_free()
	current_line = target_line
	choice_mode = false
	display_line()

func change_background(bg_name: String) -> void:
	var texture: Texture2D = load("res://assets/backgrounds/" + bg_name + ".png")
	if texture:
		background_sprite.texture = texture

func show_character(char_name: String, expression: String) -> void:
	var texture: Texture2D = load("res://assets/characters/" + char_name + "/" + expression + ".png")
	if texture:
		character_sprite.texture = texture

func _auto_advance() -> void:
	if not choice_mode and not is_typing:
		advance_line()

func end_dialogue() -> void:
	is_displaying = false
	hide()
	dialogue_ended.emit()
