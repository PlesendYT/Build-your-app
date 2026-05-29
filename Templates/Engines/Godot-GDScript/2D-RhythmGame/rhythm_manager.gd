# Rhythm game manager: spawns notes on beat, scores hits by accuracy, tracks combo
extends Node2D

@export var lanes: int = 4
@export var note_speed: float = 400.0
@export var spawn_time_offset: float = 2.0
@export var beat_interval: float = 0.5

@onready var note_scene: PackedScene = preload("res://note.tscn")
@onready var spawn_positions: Node2D = $SpawnPositions
@onready var hit_positions: Node2D = $HitPositions
@onready var audio_player: AudioStreamPlayer = $AudioStreamPlayer
@onready var spawn_timer: Timer = $SpawnTimer
@onready var score_label: Label = $UI/ScoreLabel
@onready var combo_label: Label = $UI/ComboLabel

var score: int = 0
var combo: int = 0
var max_combo: int = 0
var is_playing: bool = false
var song_data: Array[Dictionary] = []
var current_beat: int = 0

func _ready() -> void:
	spawn_timer.timeout.connect(_on_spawn_timer_timeout)

func load_song(data: Array[Dictionary]) -> void:
	song_data = data
	spawn_timer.start(beat_interval)

func start_song() -> void:
	is_playing = true
	audio_player.play()
	spawn_timer.start(beat_interval)

func _on_spawn_timer_timeout() -> void:
	if not is_playing:
		return

	for note_data in song_data:
		if note_data.get("beat") == current_beat:
			spawn_note(note_data)

	current_beat += 1

func spawn_note(data: Dictionary) -> void:
	var note: Node2D = note_scene.instantiate()
	var lane: int = data.get("lane", 0)
	note.global_position = spawn_positions.get_child(lane).global_position
	note.lane = lane
	note.speed = note_speed
	note.note_type = data.get("type", "normal")
	add_child(note)

func hit_note(lane: int, accuracy: String) -> void:
	var points: int = 0
	match accuracy:
		"perfect":
			points = 300
			combo += 1
		"good":
			points = 100
			combo += 1
		"okay":
			points = 50
			combo = 0
		"miss":
			points = 0
			combo = 0

	score += points
	max_combo = max(max_combo, combo)
	update_ui()

func update_ui() -> void:
	score_label.text = "Score: " + str(score)
	combo_label.text = "Combo: " + str(combo)

func _input(event: InputEvent) -> void:
	if not is_playing:
		return

	for i in range(lanes):
		var action: String = "lane_" + str(i + 1)
		if event.is_action_pressed(action):
			check_hit(i)

func check_hit(lane: int) -> void:
	var closest_note: Node2D = null
	var closest_distance: float = INF

	for note in get_children():
		if note is Node2D and note.has_method("get_lane"):
			if note.lane == lane and not note.is_hit:
				var dist: float = abs(note.global_position.y - hit_positions.get_child(lane).global_position.y)
				if dist < closest_distance:
					closest_distance = dist
					closest_note = note

	if closest_note == null:
		hit_note(lane, "miss")
		return

	if closest_distance < 20:
		closest_note.hit("perfect")
		hit_note(lane, "perfect")
	elif closest_distance < 40:
		closest_note.hit("good")
		hit_note(lane, "good")
	elif closest_distance < 70:
		closest_note.hit("okay")
		hit_note(lane, "okay")
	else:
		hit_note(lane, "miss")
