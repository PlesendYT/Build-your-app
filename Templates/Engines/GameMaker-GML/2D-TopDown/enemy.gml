/// @description Enemy with chase AI for top-down game

// Create event
hp = 3;
move_speed = 2;
chase_range = 250;
damage = 1;
attack_cooldown = 0;

// Step event
var _dist = distance_to_object(obj_player);
var _dir = point_direction(x, y, obj_player.x, obj_player.y);

if (_dist < chase_range) {
    // Chase player
    var _new_x = x + lengthdir_x(move_speed, _dir);
    var _new_y = y + lengthdir_y(move_speed, _dir);

    if (!place_meeting(_new_x, y, obj_wall)) {
        x = _new_x;
    }
    if (!place_meeting(x, _new_y, obj_wall)) {
        y = _new_y;
    }
} else {
    // Patrol randomly
    // (simplified: idle)
}

// Attack player on contact
if (attack_cooldown > 0) {
    attack_cooldown--;
}
if (place_meeting(x, y, obj_player) && attack_cooldown == 0) {
    with (obj_player) {
        hp -= other.damage;
    }
    attack_cooldown = 30;
}

// Take damage
if (hp <= 0) {
    instance_destroy();
}
