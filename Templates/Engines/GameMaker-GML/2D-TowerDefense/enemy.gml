/// @description Enemy for tower defense

// Create event
hp = 20;
max_hp = hp;
speed = 2;
reward = 10;
damage_to_base = 1;
waypoint_index = 0;
slow_timer = 0;
base_speed = speed;
element = "normal"; // "normal", "fast", "armored", "boss"

// Step event
if (slow_timer > 0) {
    slow_timer--;
    speed = base_speed * 0.5;
} else {
    speed = base_speed;
}

// Follow waypoints
if (waypoint_index < array_length(waypoints)) {
    var _wp = waypoints[waypoint_index];
    var _dist = point_distance(x, y, _wp.x, _wp.y);
    if (_dist > 8) {
        move_towards_point(_wp.x, _wp.y, speed);
    } else {
        waypoint_index++;
        speed = 0;
    }
} else {
    // Reached end - damage base
    obj_game_manager.lives -= damage_to_base;
    instance_destroy();
}

// Death
if (hp <= 0) {
    obj_game_manager.gold += reward;
    instance_destroy();
}

// Draw health bar
draw_set_color(c_red);
draw_rectangle(x - 16, y - 24, x + 16, y - 20, false);
draw_set_color(c_green);
var _hp_ratio = hp / max_hp;
draw_rectangle(x - 16, y - 24, x - 16 + 32 * _hp_ratio, y - 20, false);
