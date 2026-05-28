extends Area2D

@export var speed: float = 500.0
@export var damage: int = 1
@export var is_enemy_bullet: bool = false

var direction: Vector2 = Vector2.RIGHT

func _physics_process(delta: float) -> void:
	position += direction * speed * delta

func _on_body_entered(body: Node2D) -> void:
	if is_enemy_bullet and body.is_in_group("player"):
		body.take_damage()
		queue_free()
	elif not is_enemy_bullet and body.is_in_group("enemy"):
		body.take_damage(damage)
		queue_free()

func _on_visible_on_screen_notifier_2d_screen_exited() -> void:
	queue_free()
