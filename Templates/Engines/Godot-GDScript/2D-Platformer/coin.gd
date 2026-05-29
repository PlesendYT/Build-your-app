# Platformer coin that rotates, awards points, and plays a sound on collection
extends Area2D

@export var value: int = 1
@export var rotation_speed: float = 2.0

@onready var animated_sprite: AnimatedSprite2D = $AnimatedSprite2D
@onready var collect_sound: AudioStreamPlayer2D = $CollectSound

func _ready() -> void:
	animated_sprite.play("idle")

func _process(delta: float) -> void:
	rotation += rotation_speed * delta

func _on_body_entered(body: Node2D) -> void:
	if body.is_in_group("player"):
		body.collect_coin(value)
		collect_sound.play()
		hide()
		$CollisionShape2D.set_deferred("disabled", true)
		await collect_sound.finished
		queue_free()
