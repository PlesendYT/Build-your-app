extends CharacterBody3D
func _physics_process(_delta):
    var x = Input.get_axis("ui_left", "ui_right")
    var z = Input.get_axis("ui_up", "ui_down")
    velocity = Vector3(x, 0, z) * 5
    move_and_slide()