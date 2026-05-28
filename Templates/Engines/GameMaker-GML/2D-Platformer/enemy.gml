/// @description Enemy patrol AI for 2D platformer

// Create event
hp = 1;
move_dir = 1;
move_speed = 2;
patrol_left = x - 96;
patrol_right = x + 96;

// Step event
// Move in current direction
x += move_dir * move_speed;

// Reverse direction at patrol boundaries or when hitting wall
if (x >= patrol_right || x <= patrol_left || place_meeting(x + move_dir, y, obj_ground)) {
    move_dir *= -1;
    image_xscale = move_dir;
}

// Check if player is in range (aggro)
var _dist = distance_to_object(obj_player);
if (_dist < 200 && _dist > 0) {
    var _player_dir = sign(obj_player.x - x);
    move_dir = _player_dir;
    move_speed = 3;
} else {
    move_speed = 2;
}

// Destroy if hp <= 0
if (hp <= 0) {
    instance_destroy();
}
