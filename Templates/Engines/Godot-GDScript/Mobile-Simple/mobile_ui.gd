# Mobile UI overlay for touch-based games
# Manages score display, pause button, and pause menu visibility
extends CanvasLayer

@onready var score_label: Label = $ScoreLabel
@onready var pause_button: TextureButton = $PauseButton
@onready var pause_menu: Control = $PauseMenu

var score: int = 0

func _ready() -> void:
	pause_menu.hide()
	get_viewport().set_meta("mobile_ui", self)

func update_score(points: int) -> void:
	score += points
	score_label.text = "Score: " + str(score)

func _on_pause_button_pressed() -> void:
	pause_menu.visible = not pause_menu.visible
	get_tree().paused = pause_menu.visible

func _on_resume_pressed() -> void:
	pause_menu.hide()
	get_tree().paused = false

func _on_quit_pressed() -> void:
	get_tree().quit()
